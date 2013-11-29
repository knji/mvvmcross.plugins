using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping.CoordinateSystems
{
    public interface ICoordinateSystem
    {
        BoundingBox DefaultBounds { get; }
    }
}