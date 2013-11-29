namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public class LinearRing : LineString
    {
        public override GeometryType GeometryType
        {
            get { return GeometryType.LinearRing; }
        }
    }
}