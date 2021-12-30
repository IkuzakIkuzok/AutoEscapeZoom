
// (c) 2021 Kazuki KOHZUKI

using Windows.Globalization;

namespace AutoEscapeZoom.Utils.Controls
{
    internal sealed class LangItem
    {
        internal string DisplayName { get; }
        internal string Tag { get; }
        
        internal LangItem(Language language)
        {
            this.DisplayName = language.NativeName;
            this.Tag = language.LanguageTag;
        } // ctor (Language)

        override public string ToString()
            => this.DisplayName;
    } // internal sealed class LangItem
} // namespace AutoEscapeZoom.Utils.Controls
