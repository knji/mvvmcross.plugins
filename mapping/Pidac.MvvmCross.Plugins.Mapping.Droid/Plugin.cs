using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.LazyConstructAndRegisterSingleton<IMapViewController, MapquestMapViewController>();
        }
    }
}