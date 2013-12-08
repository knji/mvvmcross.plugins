using Android.Graphics;

namespace MapQuest.Android.Maps
{
    public partial class Util
    {
        public static Color Int32ToColor(int int32)
        {
            // Unpack a 32 bit 8888 ARGB integer into the destination color. The components are assumed to be tightly
            // packed in the integer as follows:
            //
            //  31-24   | 23-16 | 15-8  | 0-7
            //  A       | R     | G     | B
            return new Color((byte)(0xFF & (int32 >> 24)), (byte)(0xFF & (int32 >> 16)), (byte)(0xFF & (int32 >> 8)), (byte)(0xFF & (int32)));
        }

        public static BoundingBox CreateBoundingBoxFromRect1(Rect rect, MapView mapView)
        {
            var projection = mapView.Projection;

            GeoPoint ul = projection.FromPixels(rect.Left, rect.Top);
            GeoPoint ur = projection.FromPixels(rect.Right, rect.Top);
            GeoPoint ll = projection.FromPixels(rect.Left, rect.Bottom);
            GeoPoint lr = projection.FromPixels(rect.Right, rect.Bottom);
            var points = new GeoPoint[4];
            points[0] = ul;
            points[1] = ur;
            points[2] = ll;
            points[3] = lr;

            int l = To1E6(180.0D); int r = To1E6(-180.0D); int t = To1E6(-90.0D); int b = To1E6(90.0D);
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].LongitudeE6 < l)
                {
                    l = points[i].LongitudeE6;
                }
                if (points[i].LongitudeE6 > r)
                {
                    r = points[i].LongitudeE6;
                }
                if (points[i].LatitudeE6 > t)
                {
                    t = points[i].LatitudeE6;
                }
                if (points[i].LatitudeE6 < b)
                {
                    b = points[i].LatitudeE6;
                }
            }

            var bbox = new BoundingBox(new GeoPoint(t, l), new GeoPoint(b, r));
            return bbox;
        }


        public static Rect CreateRectFromBoundingBox1(BoundingBox bbox, MapView mapView)
        {
            var projection = mapView.Projection;

            Point ul = projection.ToPixels(new GeoPoint(bbox.Ul.LatitudeE6, bbox.Ul.LongitudeE6), null);

            Point ur = projection.ToPixels(new GeoPoint(bbox.Ul.LatitudeE6, bbox.Lr.LongitudeE6), null);

            Point ll = projection.ToPixels(new GeoPoint(bbox.Lr.LatitudeE6, bbox.Ul.LongitudeE6), null);

            Point lr = projection.ToPixels(new GeoPoint(bbox.Lr.LatitudeE6, bbox.Lr.LongitudeE6), null);

            Point[] points = new Point[4];
            points[0] = ul;
            points[1] = ur;
            points[2] = ll;
            points[3] = lr;
            int l = 0; int r = 0; int t = 0; int b = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if ((points[i].X < l) || (l == 0))
                {
                    l = points[i].X;
                }
                if ((points[i].X > r) || (r == 0))
                {
                    r = points[i].X;
                }
                if ((points[i].Y < t) || (t == 0))
                {
                    t = points[i].Y;
                }
                if ((points[i].Y > b) || (b == 0))
                {
                    b = points[i].Y;
                }
            }
            Rect image_region = new Rect(l, t, r, b);

            return image_region;
        }
    }
}