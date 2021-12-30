// (c) 2021 Kazuki KOHZUKI

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using static AutoEscapeZoom.Utils.Win32;
using Point = System.Drawing.Point;

namespace AutoEscapeZoom.Utils
{
    internal static class CaptureUtil
    {
        static CaptureUtil()
        {
            //SetProcessDPIAware();
        } // cctor ()

        private static IEnumerable<IntPtr> GetHWnds(string name)
            => Process.GetProcessesByName(name).Select(proc => proc.MainWindowHandle);

        private static IntPtr GetHWnd(string name)
        {
            var procs = GetHWnds(name);
            return procs.Any() ? procs.First() : IntPtr.Zero;
        } // private static IntPtr GetHWnd (string)

        internal static IEnumerable<MeetingData<Bitmap>> GetWindows(string name, float scaling = 1)
        {
            foreach (var hWnd in GetHWnds(name))
                yield return GetWindow(hWnd, scaling);
        } // internal static IEnumerable<MeetingData<Bitmap>> GetWindows (string, [float])

        internal static MeetingData<Bitmap> GetWindow(IntPtr hWnd, float scaling = 1)
        {
            // https://stackoverflow.com/questions/37931433/capture-screen-of-window-by-handle

            if (hWnd == IntPtr.Zero) return null;

            if (!GetWindowRect(new(null, hWnd), out var rect)) return null;

            var region = new Rectangle()
            {
                X = rect.Left,
                Y = rect.Top,
                Width = rect.Right - rect.Left,
                Height = rect.Bottom - rect.Top,
            };
            if (region.Width * region.Height == 0) return null;

            var bitmap = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);
            using var graphics = Graphics.FromImage(bitmap);
            IntPtr hdcBitmap;
            try
            {
                hdcBitmap = graphics.GetHdc();
            }
            catch
            {
                return null;
            }
            var succeeded = PrintWindow(hWnd, hdcBitmap, 0);
            graphics.ReleaseHdc(hdcBitmap);

            if (!succeeded)
            {
                graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(Point.Empty, bitmap.Size));
            }

            var hRgn = CreateRectRgn(0, 0, 0, 0);
            var reg = Region.FromHrgn(hRgn);

            if (!reg.IsEmpty(graphics))
            {
                graphics.ExcludeClip(region);
                graphics.Clear(Color.Transparent);
            }

            if (scaling == 1) return new(hWnd, bitmap);

            var w = (int)(region.Width * scaling);
            var h = (int)(region.Height * scaling);
            var scaled = new Bitmap(w, h);
            using var g = Graphics.FromImage(scaled);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bitmap, 0, 0, w, h);

            return new(hWnd, scaled);
        } // internal static MeetingData<Bitmap> GetScreen (IntPtr, [float])

        internal static MeetingData<Bitmap> GetWindow(string name)
        {
            var hWnd = GetHWnd(name);
            return GetWindow(hWnd);
        } // internal static MeetingData<Bitmap> GetScreen (string)
    } // internal static class CaptureUtil
} // namespace AutoEscapeZoom.Utils
