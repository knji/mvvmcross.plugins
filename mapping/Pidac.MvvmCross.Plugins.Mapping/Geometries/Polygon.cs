using System.Collections.ObjectModel;

namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public class Polygon : Surface
    {
        public override GeometryType GeometryType
        {
            get { return GeometryType.Polygon;}
        }

        public override bool Equals(Geometry other)
        {
            throw new System.NotImplementedException();
        }

        public override Geometry BufferBy(double d)
        {
            throw new System.NotImplementedException();
        }

        public override string AsText()
        {
            throw new System.NotImplementedException();
        }

        public override BoundingBox GetBoundingBox()
        {
            var bb = new BoundingBox();
            foreach (var ring in Rings)
            {
                bb.Union(ring.GetBoundingBox());
            }

            return bb;
        }

        public Collection<LinearRing> Rings { get; set; }
    }
}