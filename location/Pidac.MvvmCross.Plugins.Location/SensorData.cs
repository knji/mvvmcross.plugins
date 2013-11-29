using System.Collections.Generic;

namespace Pidac.MvvmCross.Plugins.Location
{
    public class SensorData
    {
        public Header Header { get; set; }

        private List<GpsData> _locations;
        public List<GpsData> Locations
        {
            get
            {
                if (_locations == null)
                    _locations = new List<GpsData>();
                return _locations;
            }
        }
    }
}