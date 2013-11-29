using System.Xml.Serialization;

namespace Pidac.MvvmCross.Plugins.Location
{
    public class Header
    {
        [XmlAttribute("version")]
        public string Version { get; set; }
    }
}