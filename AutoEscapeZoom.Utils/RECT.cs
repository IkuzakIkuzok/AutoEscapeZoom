
// (c) 2021 Kazuki KOHZUKI

using System.Runtime.InteropServices;

namespace AutoEscapeZoom.Utils
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    } // public struct RECT
} // namespace AutoEscapeZoom.Utils
