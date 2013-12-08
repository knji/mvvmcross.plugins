using System.Collections.Generic;

namespace MapQuest.Android.Maps
{
    public partial class GeoPoint : IGeometry
    {
        public GeometryType GeometryType {
            get
            {
                return GeometryType.Point;
            }
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(this, this);
        }
    }

    public abstract class Curve : Geometry
    {
        public override int Dimension { get { return 1; } }
        public virtual List<GeoPoint> Vertices { get; set; }
        public virtual GeoPoint EndPoint
        {
            get { return Vertices != null && Vertices.Count > 0 ? Vertices[Vertices.Count - 1] : null; }
        }

        public virtual GeoPoint StartPoint
        {
            get { return Vertices != null && Vertices.Count > 0 ? Vertices[0] : null; }
        }

        public bool IsClosed
        {
            get { return (StartPoint.Equals(EndPoint)); }
        }

        public override BoundingBox GetBoundingBox()
        {
            return BoundingBox.FromGeometries(Vertices.ToArray());
        }
    }
}