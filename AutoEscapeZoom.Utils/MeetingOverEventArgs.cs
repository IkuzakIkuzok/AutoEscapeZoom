
// (c) 2021 Kazuki KOHZUKI

using System;

namespace AutoEscapeZoom.Utils
{
    internal delegate void MeetingOverEventHandler(object sender, MeetingOverEventArgs e);

    internal sealed class MeetingOverEventArgs : EventArgs
    {
        internal IntPtr HWnd { get; }

        internal MeetingOverEventArgs(IntPtr hWnd)
        {
            this.HWnd = hWnd;
        } // ctor (IntPtr)
    } // internal sealed class MeetingOverEventArgs : EventArgs
} // namespace AutoEscapeZoom.Utils
