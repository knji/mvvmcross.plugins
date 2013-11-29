using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Plugins.Location;

namespace Pidac.MvvmCross.Plugins.Location
{
    public class PluginLoader : IMvxConfigurablePluginLoader
    {
        private bool _loaded;
        public static readonly PluginLoader Instance = new PluginLoader();
        private MockLocationWatcherConfiguration _configuration;

        public void EnsureLoaded()
        {
            if (_loaded)
                return;

            _loaded = true;

            var locationWatcher = new MockGeoLocationWatcher
                {
                    SensorLocationData = _configuration.SensorLocationData
                };
            Mvx.RegisterSingleton(typeof(IMvxLocationWatcher), locationWatcher);
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            _configuration = (MockLocationWatcherConfiguration) configuration;
        }
    }

    public class MockLocationWatcherConfiguration : IMvxPluginConfiguration
    {
        public static readonly MockLocationWatcherConfiguration Default = new MockLocationWatcherConfiguration();

        // ideally, we should use this property to point to a file or string containing location data
        // this should be configurable outside of code base.
        public string SensorLocationData { get; set; }
        public int UpdatePeriod { get; set; }
    }
}
