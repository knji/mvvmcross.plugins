using System;
using NUnit.Framework;
using Pidac.MvvmCross.Plugins.Location;


namespace Pidac.MvvmCross.Plugins.LocationTests
{
    [TestFixture]
    public class MockGeoLocationWatcherTests
    {
        [Test]
        public void TestInitializeSensorData()
        {
            var watcher = new MockGeoLocationWatcher();
          
            var data = @"<?xml version='1.0' encoding='utf-8'?>
<WindowsPhoneEmulator xmlns='http://schemas.microsoft.com/WindowsPhoneEmulator/2009/08/SensorData'>
    <SensorData>
        <Header version='1' />
        <GpsData latitude='48.619934106826' longitude='-84.5247359841114' />
        <GpsData latitude='48.6852544862377' longitude='-83.9864059059864' />
        <GpsData latitude='48.8445703681025' longitude='-83.7337203591114' />
        <GpsData latitude='48.8662561090809' longitude='-83.2393355934864' />
        <GpsData latitude='49.0825970371386' longitude='-83.0415816872364' />
        <GpsData latitude='49.2621642999055' longitude='-82.7229781716114' />
        <GpsData latitude='49.2621642999055' longitude='-82.6021285622364' />
        <GpsData latitude='49.2047736379815' longitude='-82.3054977028614' />
    </SensorData>
</WindowsPhoneEmulator>";
            
            watcher.InitializeSensorData(data);
            watcher.ComputeCurrentPosition();

            var currentPosition = watcher.GetLocation();
            Assert.NotNull(currentPosition);
            Assert.That(currentPosition.Coordinates.Latitude.CompareTo(48.619934106826) == 0 && currentPosition.Coordinates.Longitude.CompareTo(-84.5247359841114) == 0);
        }
    }
}
