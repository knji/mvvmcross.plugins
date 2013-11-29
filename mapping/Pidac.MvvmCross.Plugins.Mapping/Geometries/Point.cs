using System;
using Pidac.MvvmCross.Plugins.Mapping.CoordinateSystems;

namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public class Point : Geometry
    {
        public const int EarthRadiusInKm = 6367;
        public const double MilesToKm = 1.609344;
        public const double FeetToKm = 0.0003048;
        public const double MetersToKm = 1000;
        public const double NauticalMilesToKm = 1.85200;

        public float X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }

        public override GeometryType GeometryType
        {
            get { return GeometryType.Point; }
        }

        public override int Dimension
        {
            get { return 0; }
        }

        public Point(string ogcWkt)
        {
            ParseFromWkt(ogcWkt);
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point(double x, double y)
        {
            X = (float) x;
            Y = (float) y;
        }

        public double DistanceToInKm(Point another)
        {
            if (another == null)
                throw new ArgumentNullException("another");

            return DistanceByHaversineInKm(X, Y, another.X, another.Y);
        }

        public static double DistanceByHaversineInKm(double x0, double y0, double x1, double y1)
        {
            double a = Math.Pow(Math.Sin(DegToRad(y1 - y0)/2), 2) +
                       Math.Cos(DegToRad(y0))
                       *Math.Cos(DegToRad(y1))
                       *Math.Pow(Math.Sin(DegToRad(x1 - x0)/2), 2);
            double d = EarthRadiusInKm*2*Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return d;
        }

        public static double DegToRad(double degrees)
        {
            return degrees*Math.PI/180;
        }

        public override bool Equals(Geometry other)
        {
            var p1 = other as Point;
            if (p1 == null)
                return false;

            return DistanceByHaversineInKm(X, Y, p1.X, p1.Y)*1000 < 2;
            //return X.CompareTo(p1.X) == 0 && Y.CompareTo(p1.Y) == 0 && Z.CompareTo(p1.Z) == 0;
        }

        public override Geometry BufferBy(double d)
        {
           return new Point(X + d, Y + d).Validate();
        }

        public override string AsText()
        {
            return string.Format("POINT({0} {1})", X, Y);
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(X, Y, X, Y);
        }

        public void ParseFromWkt(string ogcWkt)
        {
            var gc = RemoveHeaderFromWellKnownString(ogcWkt, "POINT");
            if (gc != null)
            {
                var points = ExtractOgcPoint(gc.Wkt);
                X = points[0];
                Y = points[1];
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        public static float[] ExtractOgcPoint(string ogcWkt)
        {
            string[] parts = ogcWkt.Split(new[] {",", " "}, StringSplitOptions.RemoveEmptyEntries);
            var points = new float[parts.Length];
            points[0] = float.Parse(parts[0]);
            points[1] = float.Parse(parts[1]);

            return points;
        }
    }
}