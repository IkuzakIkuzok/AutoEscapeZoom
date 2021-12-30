
// (c) 2021 Kazuki KOHZUKI

using System;
using System.Runtime.InteropServices;

namespace AutoEscapeZoom.Utils
{
    internal static class Win32
    {
        #region Win32API Constants
        internal const int INPUT_MOUSE    = 0;
        internal const int INPUT_KEYBOARD = 1;
        internal const int INPUT_HARDWARE = 2;

        internal const int KEYEVENTF_KEYDOWN     = 0x0;
        internal const int KEYEVENTF_EXTENDEDKEY = 0x1;
        internal const int KEYEVENTF_KEYUP       = 0x2;
        internal const int KEYEVENTF_UNICODE     = 0x4;
        internal const int KEYEVENTF_SCANCODE    = 0x8;
        #endregion Win32API Constants

        #region Win32API Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUTUNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public INPUTUNION inputUnion;
        }
        #endregion Win32API Structures

        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("User32.dll")]
        internal extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("user32.dll")]
        internal static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public extern static IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
        internal static extern int MapVirtualKey(int wCode, int wMapType);
    } // internal static class Win32
} // namespace AutoEscapeZoom.Utils
