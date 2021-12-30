
// (c) 2021 Kazuki KOHZUKI

using System;

namespace AutoEscapeZoom.Utils
{
    internal sealed class MeetingState
    {
        private MeetingData<int> participants;
        private int maximum;

        internal event MeetingOverEventHandler MeetingSeemsToBeOver;

        internal MeetingData<int> Participants
        {
            get => this.participants;
            set
            {
                if (value == null)
                    value = new(IntPtr.Zero, -1);
                if (this.participants == value) return;
                if (value.Data < 0)
                {
                    Reset();
                    return;
                }
                this.participants = value;
                this.maximum = Math.Max(this.maximum, value.Data);
                if (this.participants.Data <= this.maximum * Configuration.Config.Threshold / 100)
                {
                    MeetingSeemsToBeOver?.Invoke(this, new(value.HWnd));
                    Reset();
                }
            }
        }

        internal void Reset()
        {
            this.participants = null;
            this.maximum = -1;
        } // internal void Reset ()
    } // internal sealed class MeetingState
} // namespace AutoEscapeZoom.Utils
