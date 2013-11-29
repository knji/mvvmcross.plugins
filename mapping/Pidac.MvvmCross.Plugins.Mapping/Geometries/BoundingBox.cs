

using System;
using System.Collections.Generic;

namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public class BoundingBox 
    {
        public static readonly BoundingBox Empty = new BoundingBox{IsEmpty = true};

        public bool IsEmpty { get; private set; }
        public Point SouthWest { get; set; }
        public Point NorthEast { get; set; }
        protected double YMax
        {
            get { return NorthEast.Y; }
        }

        protected double XMax
        {
            get
            {
                return NorthEast.X;
            }
        }

        protected double YMin
        {
            get { return SouthWest.Y; }
        }

        protected double XMin
        {
            get { return SouthWest.X; }
        }
        public BoundingBox() : this(-180, -90, 180, 90)
        {
        }

        public BoundingBox(IList<Geometry> geometries)
        {
            var bb = new BoundingBox(geometries[0].GetBoundingBox());
            for (var i = 1; i < geometries.Count; i++)
            {
                bb.Union(geometries[i].GetBoundingBox());
            }

            SouthWest = bb.SouthWest;
            NorthEast = bb.NorthEast;
        }

        public BoundingBox(double xmin, double ymin, double xmax, double ymax)
        {
            SouthWest = new Point(xmin, ymin);
            NorthEast = new Point(xmax, ymax);
        }

        public BoundingBox(Point southWest, Point northEast)
        {
            SouthWest = southWest;
            NorthEast = northEast;
        }

        public BoundingBox(BoundingBox bbox)
        {
            SouthWest = bbox.SouthWest;
            NorthEast = bbox.NorthEast;
        }

        public double GetHeight()
        {
            // todo: user a coordinate system
            return Math.Abs(YMax - YMin); 
        }

        public double GetWidth()
        {
            // todo: user a coordinate system
            return Math.Abs(XMax - XMin);
        }

        public double GetArea()
        {
            return GetHeight()*GetWidth();
        }


        public bool Equals(BoundingBox other)
        {
            if (other == null) return false;
            return other.SouthWest.Equals(SouthWest) && other.NorthEast.Equals(NorthEast);
        }

        public BoundingBox Union(BoundingBox other)
        {
            var bbox = other.GetBoundingBox();

            SouthWest.X = Math.Min(SouthWest.X, bbox.SouthWest.X);
            SouthWest.Y = Math.Min(SouthWest.Y, bbox.SouthWest.Y);
            NorthEast.X= Math.Max(NorthEast.X, bbox.NorthEast.X);
            NorthEast.Y = Math.Max(NorthEast.Y, bbox.NorthEast.Y);
            return this;
        }

        public string AsText()
        {
            return string.Format("{0},{1},{2},{3}", XMin, YMin, XMax, YMax);
        }

        public BoundingBox GetBoundingBox()
        {
            return this;
        }

        public static BoundingBox GetBounds(IList<Point> points)
        {
            if ((points == null) || (points.Count == 0))
            {
                throw new ArgumentException("points");
            }

            var bbox = new BoundingBox(points[0], points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                bbox.SouthWest.X = Math.Min(points[i].X, bbox.SouthWest.X);
                bbox.SouthWest.Y = Math.Min(points[i].Y, bbox.SouthWest.Y);
                bbox.NorthEast.X = Math.Max(points[i].X, bbox.NorthEast.X);
                bbox.NorthEast.Y = Math.Max(points[i].Y, bbox.NorthEast.Y);
            }
            return bbox;
        }

        public override string ToString()
        {
            return string.Format("Xmin={0} Ymin={1} Xmax={2} Ymax={3}", XMin, YMin, XMax, YMax);
        }

        public BoundingBox BufferBy(float amount)
        {
            SouthWest.X -= amount;
            SouthWest.Y -= amount;
            NorthEast.X += amount;
            NorthEast.Y += amount;

            return this;
        }

        public BoundingBox Validate()
        {
            SouthWest = (Point)SouthWest.Validate();
            NorthEast = (Point)NorthEast.Validate();
            return this;
        }
    }
}