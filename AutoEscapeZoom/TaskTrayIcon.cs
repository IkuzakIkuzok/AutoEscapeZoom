
// (c) 2021 Kazuki KOHZUKI

using AutoEscapeZoom.Utils.Controls;
using System;
using System.Windows.Forms;

namespace AutoEscapeZoom
{
    internal sealed class TaskTrayIcon
    {
        private readonly NotifyIcon _base;
        private readonly ToolStripMenuItem toggle;

        internal TaskTrayIcon()
        {
            this._base = new()
            {
                Text = "AutoEscapeZoom",
                Icon = Properties.Resources.Icon,
                Visible = true,
            };
            this._base.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    ShowConfigForm();
            };
            this._base.ContextMenuStrip = new()
            {
                ShowItemToolTips = true,
            };

            void Add(ToolStripItem item)
                => this._base.ContextMenuStrip.Items.Add(item);

            this.toggle = new()
            {
                Text = "停止 (&S)",
            };
            this.toggle.Click += Toggle;
            Add(this.toggle);

            var config = new ToolStripMenuItem()
            {
                Text = "設定 (&C)"
            };
            config.Click += ShowConfigForm;
            Add(config);

            var exit = new ToolStripMenuItem()
            {
                Text = "終了 (&X)",
            };
            exit.Click += Exit;
            Add(exit);
        } // ctor ()

        private static void ShowConfigForm()
            => new ConfigForm().Show();

        private static void ShowConfigForm(object sender, EventArgs e)
            => ShowConfigForm();

        private void Toggle(object sender, EventArgs e)
        {
            Kernel.Toggle();
            if (Kernel.Running)
            {
                this._base.Text = "AutoEscapeZoom (実行中)";
                this.toggle.Text = "停止 (&S)";
            }
            else
            {
                this._base.Text = "AutoEscapeZoom (停止中)";
                this.toggle.Text = "再開 (&S)";
            }
        } // private void Toggle (object, EventArgs)

        internal void Hide()
            => this._base.Visible = false;

        private void Exit(object sender, EventArgs e)
        {
            Hide();
            Kernel.Exit();
        } // private void Exit (object, EventArgs)
    } // internal sealed class TaskTrayIcon
} // namespace AutoEscapeZoom
