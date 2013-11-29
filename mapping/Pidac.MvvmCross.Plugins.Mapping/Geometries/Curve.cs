using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public abstract class Curve : Geometry
    {
        public override int Dimension { get { return 1; } }
        public virtual List<Point> Vertices { get; set; }
        public virtual Point EndPoint
        {
            get { return Vertices != null && Vertices.Count > 0 ? Vertices[Vertices.Count - 1] : null; }
        }

        public virtual Point StartPoint
        {
            get { return Vertices != null && Vertices.Count > 0 ? Vertices[0] : null; }
        }

        public bool IsClosed
        {
            get { return (StartPoint.Equals(EndPoint)); }
        }

        public override BoundingBox GetBoundingBox()
        {
            return BoundingBox.GetBounds(Vertices);
        }
    }
}