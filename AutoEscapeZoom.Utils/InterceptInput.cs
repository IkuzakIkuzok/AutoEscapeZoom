
// (c) 2021 Kazuki KOHZUKI

using System.Runtime.InteropServices;
using static AutoEscapeZoom.Utils.Win32;

namespace AutoEscapeZoom.Utils
{
    internal static class InterceptInput
    {
        internal const int MAGIC_NUMBER = 0x1123;

        internal static INPUT KeyDown(int key, bool isExtend = false)
        {
            var input = new INPUT
            {
                type = INPUT_KEYBOARD,
                inputUnion = new()
                {
                    ki = new()
                    {
                        wVk = (short)key,
                        wScan = (short)MapVirtualKey((short)key, 0),
                        dwFlags = (isExtend ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYDOWN,
                        time = 0,
                        dwExtraInfo = System.IntPtr.Zero,
                    }
                }
            };

            SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
            return input;
        } // internal static INPUT KeyDown(int, [bool])

        internal static void KeyUp(INPUT input, bool isExtend = false)
        {
            input.inputUnion.ki.dwFlags = (isExtend ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, new[] { input }, Marshal.SizeOf(input));
        } // internal static void KeyUp(INPUT, [bool])

        internal static void SendKey(int key, bool isExtend = false)
        {
            var input = KeyDown(key, isExtend);
            KeyUp(input, isExtend);
        } // internal static void SendKey(int, [bool])
    } // internal static class InterceptInput
} // namespace AutoEscapeZoom.Utils
