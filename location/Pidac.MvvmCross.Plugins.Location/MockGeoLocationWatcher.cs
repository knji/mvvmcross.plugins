using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Cirrious.MvvmCross.Plugins.Location;

namespace Pidac.MvvmCross.Plugins.Location
{
    public class MockGeoLocationWatcher : IMvxLocationWatcher
    {
        private DateTime _lastUpdate = DateTime.UtcNow;
        private Timer _updateTimer;
        private MvxLocationOptions _options;
        private bool _isStarted;
        private Action<MvxGeoLocation> _onSuccess;
        private MvxGeoLocation _currentLocation;
        private Action<MvxLocationError> _onError;
        private SensorData _sensorData;
        private int _currentLocationIndex = -1;
        
        public string SensorLocationData { get; set; }

        public void Start(MvxLocationOptions options, Action<MvxGeoLocation> success, Action<MvxLocationError> error)
        {
            if (string.IsNullOrWhiteSpace(SensorLocationData))
                throw new ArgumentException("SensorLocationData has not yet been initialized. ");

            if (success == null)
                throw new ArgumentNullException("success");
            _onSuccess = success;

            _onError = error;
            _isStarted = true;
            _currentLocationIndex = -1;
            _options = options ?? new MvxLocationOptions();

            InitializeSensorData(SensorLocationData);
            
            if (_updateTimer == null)
            {
                _updateTimer = new Timer(arg =>
                {
                    var location = ComputeCurrentPosition();
                    CurrentLocation = location;
                    LastSeenLocation = location;
                    _onSuccess.Invoke(location);
                }, 
                null, 0,  (int) _options.TimeBetweenUpdates.TotalMilliseconds);
            }
            else
            {
                _updateTimer.Change(0, (int)_options.TimeBetweenUpdates.TotalMilliseconds);
            }
        }

        public void InitializeSensorData(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentNullException("data");

            XNamespace ns = "http://schemas.microsoft.com/WindowsPhoneEmulator/2009/08/SensorData";
            var root = XElement.Parse(data);
            var header = new Header();
            
            var sensorDataNode = root.Element(ns + "SensorData");
            if (sensorDataNode == null) return;

            var headerNode = sensorDataNode.Element(ns + "Header");
            if (headerNode != null)
            {
                var xAttribute = headerNode.Attribute("version");
                if (xAttribute != null) header.Version = xAttribute.Value;
            }

            var gpsData = sensorDataNode.Elements(ns + "GpsData").Select(d =>
            {
                var latAtribute = d.Attribute("latitude");
                var lonAttribute = d.Attribute("longitude");
                return new GpsData
                {
                    Latitude = latAtribute != null ? Convert.ToDouble(latAtribute.Value) : 0,
                    Longitude = lonAttribute != null ? Convert.ToDouble(lonAttribute.Value) : 0
                };
            }).ToArray();

            if (_sensorData == null)
                _sensorData = new SensorData {Header = header};
            else
                _sensorData.Locations.Clear();

            _sensorData.Locations.AddRange(gpsData);
        }

        public MvxGeoLocation ComputeCurrentPosition()
        {
            _lastUpdate = DateTime.UtcNow;
            _currentLocationIndex = _currentLocationIndex + 1;

            var location = _sensorData.Locations[_currentLocationIndex];
            _currentLocation = ConvertToMvxGeoLocation(location);
            return _currentLocation;
        }

        private MvxGeoLocation ConvertToMvxGeoLocation(GpsData gpsData)
        {
            var coordinates = new MvxCoordinates {
                Latitude = gpsData.Latitude, 
                Longitude = gpsData.Longitude,
                Speed = ComputeSpeed(),
                Heading = ComputeHeading()
            };

            return new MvxGeoLocation {Coordinates = coordinates};
        }

        private double? ComputeHeading()
        {
            // TODO: 
            return 0;
        }

        private double? ComputeSpeed()
        {
            //TODO: compute from distance travelled/update time...
            return 0;
        }

        public void Stop()
        {
            _isStarted = false;
            if (_updateTimer != null)
                _updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public MvxGeoLocation GetLocation()
        {
            return _currentLocation;
        }

        public bool Started 
        {
            get { return _isStarted; }
        }

        public MvxGeoLocation CurrentLocation { get; private set; }
        public MvxGeoLocation LastSeenLocation { get; private set; }
    }
}
