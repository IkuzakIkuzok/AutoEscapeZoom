
// (c) 2021 Kazuki KOHZUKI

using PandA.Plugins.Utils;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AutoEscapeZoom.Utils
{
    [Serializable]
    [XmlRoot("config")]
    public sealed class Configuration
    {
        internal static Configuration Config { get; } = Load();

        private static string path;

        private static string FilePath
        {
            get
            {
                return path ??= Path.Combine(
                    CommonData.PluginConfigDirectoryName,
                    "AutoEscapeZoom",
                    "config.xml"
                );
            }
        }

        [XmlElement("threshold")]
        public int Threshold { get; set; } = 60;

        [XmlElement("interval")]
        public uint Interval { get; set; } = 10;

        public Configuration() { }

        private static Configuration Load()
        {
            try
            {
                using var sr = new StreamReader(FilePath, Encoding.UTF8, true);
                var cs = new ConfigSerializer();
                return cs.Deserialize(sr);
            }
            catch
            {
                return new();
            }
        } // private static Configuration Load ()

        internal void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            using var sw = new StreamWriter(FilePath, false, Encoding.UTF8);
            var cs = new ConfigSerializer();
            cs.Serialize(sw, this);
        } // internal void Save ()
    } // public sealed class Configuration
} // namespace AutoEscapeZoom.Utils
