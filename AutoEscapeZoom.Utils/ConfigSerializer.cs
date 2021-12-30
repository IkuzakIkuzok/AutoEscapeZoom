
// (c) 2021 Kazuki KOHZUKI

using System.IO;
using System.Xml.Serialization;

namespace AutoEscapeZoom.Utils
{
    internal class ConfigSerializer : XmlSerializer
    {
        internal ConfigSerializer() : base(typeof(Configuration)) { }

        new internal Configuration Deserialize(TextReader textReader)
            => (Configuration)base.Deserialize(textReader);

        internal void Serialize(TextWriter textWriter, Configuration configuration)
            => base.Serialize(textWriter, configuration);
    } // internal sealed class ConfigSerializer : XmlSerializer
} // namespace AutoEscapeZoom.Utils
