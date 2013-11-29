using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping.CoordinateSystems
{
    public class GeographicCoordinateSystem : CoordinateSystem
    {
        public GeographicCoordinateSystem(string name, string authority, int authorityCode)
            : base(name, authority, authorityCode)
        {
            DefaultBounds = new BoundingBox(-180, -90, 180, 90);
        }

        public static readonly GeographicCoordinateSystem Wgs84 = new GeographicCoordinateSystem("WGS 84", "EPSG", 4326);
    }
}