using System.Xml.Serialization;

namespace Pidac.MvvmCross.Plugins.Location
{
    public class GpsData
    {
        [XmlAttribute("latitude")]
        public double Latitude { get; set; }
        [XmlAttribute("latitude")]
        public double Longitude { get; set; }

        public override string ToString()
        {
            return Latitude + ", " + Longitude;
        }
    }
}