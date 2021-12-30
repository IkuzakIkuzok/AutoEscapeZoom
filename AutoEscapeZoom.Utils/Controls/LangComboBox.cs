
// (c) 2021 Kazuki KOHZUKI

using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoEscapeZoom.Utils.Controls
{
    [DesignerCategory("Code")]
    internal sealed class LangComboBox : ComboBox
    {
        new internal IEnumerable<LangItem> Items
        {
            get
            {
                foreach (var item in base.Items.Cast<LangItem>())
                    yield return item;
            }
        }

        internal void Add(LangItem item)
            => base.Items.Add(item);
    } // internal sealed class LangComboBox : ComboBox
} // namespace AutoEscapeZoom.Utils.Controls
