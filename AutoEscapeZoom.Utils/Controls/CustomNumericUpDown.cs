
// (c) 2021 Kazuki KOHZUKI

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AutoEscapeZoom.Utils.Controls
{
    [DesignerCategory("Code")]
    internal sealed class CustomNumericUpDown : NumericUpDown
    {
        private int _abs_increment, _increment;
        private int _abs_scroll_increment, _scroll_increment;

        #region properties

        /// <summary>
        /// Gets or sets the value to increment or decrement the spin box (also known as an up-down control) when the up or down buttons are clicked.
        /// </summary>
        new internal int Increment
        {
            set
            {
                this._increment = value;
                this._abs_increment = Math.Abs(value);
            }
            get => this._increment;
        }

        /// <summary>
        /// Gets or sets the value to increment or decrement the spin box (also known as an up-down control) when th mousee wheel spined.
        /// </summary>
        internal int ScrollIncrement
        {
            set
            {
                this._scroll_increment = value;
                this._abs_scroll_increment = Math.Abs(value);
            }
            get => this._scroll_increment;
        }

        internal int ValueAsInt
        {
            set => this.Value = value;
            get => (int)this.Value;
        }


        #endregion properties

        internal CustomNumericUpDown() : base()
        {
            this.Increment = 1;
            this.ScrollIncrement = 1;
            this.ImeMode = ImeMode.Disable;
        } // ctor ()

        new internal void UpButton() => UpDown(this._increment > 0);

        new internal void DownButton() => UpDown(this._increment < 0);

        private void UpDown(bool up)
        {
            try
            {
                this.Value = up
                    ? Math.Min(checked(this.Value + this._abs_increment), this.Maximum) // increment
                    : Math.Max(checked(this.Value - this._abs_increment), this.Minimum) // decrement
                    ;
            }
            catch (OverflowException)
            {
                this.Value = up ? this.Maximum : this.Minimum;
            }
        } // private void UpDown (bool)

        override protected void OnMouseWheel(MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs hme) hme.Handled = true;

            var up = e.Delta > 0 ^ this._scroll_increment > 0;
            try
            {
                this.Value = up
                    ? Math.Max(checked(this.Value - this._abs_scroll_increment), this.Minimum) // decrement
                    : Math.Min(checked(this.Value + this._abs_scroll_increment), this.Maximum) // increment
                    ;
            }
            catch (OverflowException)
            {
                this.Value = up ? this.Maximum : this.Minimum;
            }
        } // override protected void OnMouseWheel (MouseEventArgs)
    } // internal sealed class CustomNumericUpDown
} // namespace AutoEscapeZoom.Utils.Controls
