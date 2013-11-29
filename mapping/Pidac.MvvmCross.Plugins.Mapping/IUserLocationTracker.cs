using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public interface IUserLocationTracker : IGeoDataManager
    {
        Point GetLatestLocation();
        Feature GetLatestLocationFeature();
        Feature GetLatestCompassFeature();
    }
}