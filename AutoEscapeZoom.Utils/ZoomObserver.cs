
// (c) 2021 Kazuki KOHZUKI

using AutoEscapeZoom.Utils.Controls;
using PandA.Plugins.Interface;
using PandA.Plugins.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Globalization;
using Timer = System.Timers.Timer;

namespace AutoEscapeZoom.Utils
{
    public class ZoomObserver : IPlugin, IAutoRun
    {
        private const int Alt = 164;
        private const int Enter = 13;

        private const float SCALING = 2.236f;

        private const string MUTEX_NAME = @"Auto_Escape_Zoom";

        private static Mutex mutex;

        private static readonly Regex re_ws = new(@"\s");
        private static readonly Regex re_participants1 = new(@"(\d+)参加者退出");
        private static readonly Regex re_participants2 = new(@"参加者[(（](\d+)[)）]");

        private StreamWriter log;
        private Timer timer;

        private static OcrEngine ocrEngine = new();
        private static readonly MeetingState meetingState = new();

        static ZoomObserver()
        {
            meetingState.MeetingSeemsToBeOver += EscapeMeeting;
        } // cctor ()

        #region properties

        internal static Language CurrentLanguage
        {
            get => ocrEngine.RecognizerLanguage;
            set
            {
                if (ocrEngine.RecognizerLanguage == value) return;
                ocrEngine = new OcrEngine(value);
            }
        }

        #endregion properties

        public ZoomObserver() { }

        async private Task<int> GetParticipants(Bitmap bitmap)
        {
            if (bitmap == null) return -1;

            var text = await ocrEngine.GetString(bitmap);
            text = re_ws.Replace(text, string.Empty);
            if (text.Contains("Zoomクラウドミーティング")) return 0;
            var mc = re_participants1.Matches(text);
            if (mc.Count == 0)
            {
                mc = re_participants2.Matches(text);
                if (mc.Count == 0) return -1;
            }
            return int.Parse(mc[0].Groups[1].Value);
        } // async private Task<int> GetParticipants (Bitmap)

        async private Task<MeetingData<int>> GetParticipants()
        {
            var bmps = CaptureUtil.GetWindows("zoom", SCALING).Where(bmp => bmp != null);
            foreach (var bmp in bmps)
            {
                var p = await GetParticipants(bmp.Data);
                if (p > 0)
                    return new(bmp.HWnd, p);
                else if (p == 0)
                    continue;

                ToggleParticipants(bmp.HWnd);
                var tmp = CaptureUtil.GetWindow(bmp.HWnd, SCALING);
                p = await GetParticipants(tmp.Data);
                if (p < 0)
                {
                    //ToggleParticipants(bmp.HWnd);
                    continue;
                }
                return new(bmp.HWnd, p);
            }
            return null;
        } // async private Task<int> GetParticipants ()

        async private Task CheckMeetingState()
            => meetingState.Participants = await GetParticipants();

        private static void EscapeMeeting(object sender, MeetingOverEventArgs e)
        {
            Notification.ShowNotification("ミーティングを退出します。", "AutoEscapeZoom");

            Win32.SetForegroundWindow(e.HWnd);
            var input = InterceptInput.KeyDown(Alt);
            InterceptInput.SendKey('Q');
            InterceptInput.KeyUp(input);
            InterceptInput.SendKey(Enter);
        } // private static void EscapeMeeting (object, MeetingOverEventArgs)

        private static void ToggleParticipants(IntPtr hWnd)
        {
            Win32.SetForegroundWindow(hWnd);
            var input = InterceptInput.KeyDown(Alt);
            InterceptInput.SendKey('U');
            InterceptInput.KeyUp(input);
        } // private static void ToggleParticipants (IntPtr)

        private void FlushLog(object sender, EventArgs e)
            => this.log?.Flush();

        #region plugin

        public string Name => "AutoEscapeZoom";

        public string Description => "Zoomから自動的に退出します。";

        public IEnumerable<ToolStripItem> Menu
        {
            get
            {
                var config = new ToolStripMenuItem()
                {
                    Text = "設定 (&C)"
                };
                config.Click += (sender, e) => new ConfigForm().Show();
                yield return config;
            }
        }

        public Icon Icon => Properties.Resources.Icon;

        public uint PeriodSeconds => Configuration.Config.Interval;

        public void Initialize()
        {
            mutex = new(true, MUTEX_NAME, out var created);
            if (!created)
                throw new InvalidProgramException("AutoEscapeZoom is already running.");

            var path = Path.Combine(
                CommonData.PluginConfigDirectoryName,
                "AutoEscapeZoom",
                "log.txt"
            );
            this.log = new(path, false, Encoding.UTF8);
            this.log.WriteLine("AutoEscapeZoom is loaded.");

            this.timer = new()
            {
                AutoReset = true,
                Interval = 1000,
            };
            this.timer.Start();
            this.timer.Elapsed += FlushLog;
        } // public void Initialize ()

        public void Terminate()
        {
            this.log?.Close();
            mutex.ReleaseMutex();
        } // public void Terminate ()

        public void Procedure()
            => _ = CheckMeetingState();

        #endregion plugin
    } // public class ZoomObserver : IPlugin, IAutoRun
} // namespace AutoEscapeZoom.Utils
