
// (c) 2021 Kazuki KOHZUKI

using System;
using System.Drawing;

namespace AutoEscapeZoom.Utils
{
    internal sealed class MeetingData<T>
    {
        internal IntPtr HWnd { get; }
        internal T Data { get; }

        internal MeetingData(IntPtr hWnd, T window)
        {
            this.HWnd = hWnd;
            this.Data = window;
        } // ctor (IntPtr, Bitmap)
    } // internal sealed class MeetingData
} // namespace AutoEscapeZoom.Utils
