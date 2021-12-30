
// (c) 2021 Kazuki KOHZUKI

using AutoEscapeZoom.Utils;
using System;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace AutoEscapeZoom
{
    internal static class Kernel
    {
        private static Timer timer;
        private static ZoomObserver observer;
        private static readonly TaskTrayIcon taskTrayIcon = new();
        private static Form form;

        internal static bool Running
            => timer.Enabled;

        [STAThread]
        private static void Main()
        {
            try
            {
                try
                {
                    observer = new();
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show(
                        "AutoEscapeZoomは既に起動しています。",
                        "AutoEscapeZoom",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                    );
                }
                observer.Initialize();

                timer = new Timer()
                {
                    Interval = 10_000,
                    AutoReset = true,
                    Enabled = true,
                };
                timer.Elapsed += (sender, e) => observer.Procedure();

                form = new()
                {
                    Visible = false,
                    Size = new(0, 0),
                };
                Application.Run();
            }
            finally
            {
                taskTrayIcon?.Hide();
            }
        } // private static void Main ()

        internal static void Toggle()
        {
            if (timer.Enabled)
                Stop();
            else
                Start();
        } // internal static void Toggle ()

        private static void Stop()
            => timer.Stop();

        private static void Start()
            => timer.Start();

        internal static void Exit()
        {
            observer.Terminate();
            form.Close();
            Application.Exit();
        } // internal static void Exit ()
    } // internal static class Kernel
} // namespace AutoEscapeZoom
