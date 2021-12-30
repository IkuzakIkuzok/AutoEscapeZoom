
// (c) 2021 Kazuki KOHZUKI

using AutoEscapeZoom.Utils.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AutoEscapeZoom.Utils.Controls
{
    [DesignerCategory("Code")]
    public sealed class ConfigForm : Form
    {
        //private readonly LangComboBox cb_lang;
        private readonly CustomNumericUpDown threshold, interval;

        public ConfigForm()
        {
            this.Text = "AutoEscapeZoom";
            this.Size = this.MinimumSize = this.MaximumSize = new(340, 320);
            this.MaximizeBox = false;
            this.Icon = Resources.Icon;

            /*_ = new Label()
            {
                Text = "言語",
                Top = 20,
                Left = 20,
                Width = 40,
                Parent = this,
            };

            this.cb_lang = new()
            {
                Top = 20,
                Left = 60,
                Width = 200,
                Parent = this,
            };
            foreach (var lang in OcrEngine.AvailableRecognizerLanguages)
                this.cb_lang.Add(new LangItem(lang));
            this.cb_lang.SelectedIndex = this.cb_lang.Items.ToList().FindIndex(x => x.Tag == ZoomObserver.CurrentLanguage.LanguageTag);*/

            _ = new Label()
            {
                Text = "変動限界 (%)",
                Top = 20,
                Left = 20,
                Width = 60,
                Parent = this,
            };

            this.threshold = new()
            {
                Top = 18,
                Left = 80,
                Width = 150,
                Maximum = 100,
                Minimum = 0,
                ValueAsInt = Configuration.Config.Threshold,
                Increment = 10,
                ScrollIncrement = 10,
                Parent = this,
            };

            _ = new Label()
            {
                Text = "参加人数がピーク時参加人数×変動限界以下になった時に退出します。",
                Top = 50,
                Left = 20,
                Size = new(250, 50),
                Parent = this,
            };

            _ = new Label()
            {
                Text = "確認頻度 (秒)",
                Top = 120,
                Left = 20,
                Width = 60,
                Parent = this,
            };

            this.interval = new()
            {
                Top = 118,
                Left = 80,
                Width = 150,
                Maximum = 60 * 60,
                Minimum = 1,
                Value = Configuration.Config.Interval,
                Increment = 10,
                ScrollIncrement = 10,
                Parent = this,
            };

            _ = new Label()
            {
                Text = "Zoomの参加人数を確認する頻度を設定します。",
                Top = 150,
                Left = 20,
                Size = new(250, 50),
                Parent = this,
            };

            var cancel = new Button()
            {
                Text = "キャンセル",
                Top = 220,
                Left = 20,
                Size = new(80, 35),
                Parent = this,
            };
            cancel.Click += (sender, e) => Close();

            var apply = new Button()
            {
                Text = "適用",
                Top = 220,
                Left = 120,
                Size = new(80, 35),
                Parent = this,
            };
            apply.Click += Apply;

            var ok = new Button()
            {
                Text = "OK",
                Top = 220,
                Left = 220,
                Size = new(80, 35),
                Parent = this,
            };
            ok.Click += (sender, e) =>
            {
                Apply();
                Close();
            };
        } // ctor ()

        private void Apply(object sender, EventArgs e)
            => Apply();

        private void Apply()
        {
            Configuration.Config.Threshold = this.threshold.ValueAsInt;
            Configuration.Config.Interval = (uint)this.interval.Value;
            Configuration.Config.Save();
        } // private void Apply ()
    } // public sealed class ConfigForm : Form
} // namespace AutoEscapeZoom.Utils.Controls
