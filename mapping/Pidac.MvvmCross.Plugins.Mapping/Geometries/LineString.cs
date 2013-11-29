using System;
using System.Collections.Generic;
using System.Linq;

namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public class LineString : Curve
    {
        public LineString()
        {
            Init(null);
        }

        public LineString(IList<Point> vertices )
        {
            Init( vertices );
        }

        private void Init(IList<Point> vertices )
        {
            if (vertices == null || vertices.Count() < 2)
                throw new ArgumentException("vertices being passed to create line is not valid.");

            Vertices = new List<Point>(vertices);
        }

        public override GeometryType GeometryType
        {
            get { return GeometryType.LineString; }
        }

        public override bool Equals(Geometry other)
        {
            var line = other as LineString;
            if (line == null) return false;
            if (line.Vertices == null || line.Vertices.Count == 0)
                return false;

            return !Vertices.Where((t, i) => !t.Equals(line.Vertices[i])).Any();
        }

        public override Geometry BufferBy(double d)
        {
            throw new System.NotImplementedException();
        }

        public override string AsText()
        {
            throw new System.NotImplementedException();
        }
    }
}