using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using MapQuest.Android.Maps;
using Double = System.Double;
using String = System.String;

namespace MapQuest.Android.Maps.Core
{
  
    public class MapView : ViewGroup
    {
         private static  String LOG_TAG = "mq.android.maps.mapview";
        private static  int MAP_VIEW_BACKGROUND = Color.Rgb(238, 238, 238);
        private static  float GUARANTEE_TILELOAD_PERCENTAGE = 0.5F;
        private static  int PRELOAD = 31459;

        private IProjection _rotatableProjection;
        private double _currentScale = 1.0F;
        private bool _scaling = false;
        private Point _scalePoint = new Point();

        public class MapViewLayoutParams : LayoutParams
        {
            public static int BOTTOM = 32;
            public static int CENTER_HORIZONTAL = 1;
            public static int CENTER_VERTICAL = 2;
            public static int LEFT = 4;
            public static int RIGHT = 8;
            public static int TOP = 16;
            public static int TOP_LEFT = 20;
            public static int CENTER = 3;
            public static int BOTTOM_CENTER = 33;
            public static int MODE_MAP = 0;
            public static int MODE_VIEW = 1;
            public int Alignment = 3;
            public int Mode = 1;
            public GeoPoint Point;
            public int X = int.MaxValue;
            public int Y = int.MaxValue;

            public MapViewLayoutParams(Context context, IAttributeSet attrs)
                : base(context, attrs)
            {
                this.X = attrs.GetAttributeIntValue("http://schemas.mapquest.com/apk/res/mapquest", "x", int.MaxValue);
                this.Y = attrs.GetAttributeIntValue("http://schemas.mapquest.com/apk/res/mapquest", "x", int.MaxValue);
                String geoPoint = attrs.GetAttributeValue("http://schemas.mapquest.com/apk/res/mapquest", "geoPoint");
                if ((geoPoint.Length > 0))
                {
                    String[] arr = geoPoint.Split(new[] { "," }, StringSplitOptions.None);
                    if (arr.Length > 1)
                    {
                        try
                        {
                            double latitude = Double.Parse(arr[0].Trim());
                            double longitude = Double.Parse(arr[1].Trim());
                            this.Point = new GeoPoint(latitude, longitude);
                            this.Mode = 0;
                        }
                        catch (NumberFormatException nfe)
                        {
                            Log.Error("mq.android.maps.mapview", "Invalid value for geoPoint attribute : " + geoPoint);
                        }
                    }
                }
            }

            public MapViewLayoutParams(LayoutParams source)
                : base(source)
            {
                var lp = source as MapViewLayoutParams;
                if (lp != null)
                {
                    this.X = lp.X;
                    this.Y = lp.Y;
                    this.Point = lp.Point;
                    this.Mode = lp.Mode;
                    this.Alignment = lp.Alignment;
                }
            }

            public MapViewLayoutParams(int width, int height, GeoPoint point, int alignment)
                : base(width, height)
            {
                this.Point = point;
                this.Alignment = alignment;
                if (point != null)
                {
                    this.Mode = 0;
                }
            }

            public MapViewLayoutParams(int width, int height, GeoPoint point, int x, int y, int alignment)
                : base(width, height)
            {
                this.Point = point;
                this.X = x;
                this.Y = y;
                this.Alignment = alignment;
                if (point != null)
                {
                    this.Mode = 0;
                }
            }

            public MapViewLayoutParams(int width, int height, int x, int y, int alignment)
                : base(width, height)
            {
                this.X = x;
                this.Y = y;
                this.Alignment = alignment;
            }
        }

        public MapView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MapView(Context context) : base(context)
        {
        }

        public MapView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MapView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            RedoLayout(changed, l, t, r, b);
        }

        private Point ScalePoint(int x, int y, Point point)
        {
            if (_scaling)
            {
                Point scalePoint = _scalePoint;
                point.Y = ((int)(scalePoint.Y + (y - scalePoint.Y) * _currentScale + 0.5F));
                point.X = ((int)(scalePoint.X + (x - scalePoint.X) * _currentScale + 0.5F));
            }
            else
            {
                point.X = x;
                point.Y = y;
            }
            return point;
        }

        private void RedoLayout(bool changed, int l, int t, int r, int b)
        {
            int c = ChildCount;
            Point point = new Point();
            int height = Height;
            int width = Width;

            for (int i = 0; i < c; i++)
            {
                View view = GetChildAt(i);
                var lp = view.LayoutParameters as MapViewLayoutParams;
                if (view.Visibility != ViewStates.Invisible /* != 8 */ && lp != null)
                {
                    if (lp.Mode == 0)
                    {
                        if (lp.Point == null)
                        {
                            Log.Error("mq.android.maps.mapview", "View instance mode is set to map but geopoint is not set");

                            point.X = lp.X;
                            point.Y = lp.Y;
                        }
                        else
                        {
                            point = Projection.ToPixels(lp.Point, point);
                            if (_currentScale.CompareTo(1.0F) != 0)
                            {
                                point = ScalePoint(point.X, point.Y, point);
                            }
                            point.X += (lp.X != int.MaxValue ? lp.X : 0);
                            point.Y += (lp.Y != int.MaxValue ? lp.Y : 0);
                        }
                    }
                    else
                    {
                        point.X = lp.X;
                        point.Y = lp.Y;
                    }
                    int alignment = lp.Alignment;
                    int cw = view.MeasuredWidth;
                    int childHeight = view.MeasuredHeight;

                    int childLeft = point.X != int.MaxValue ? point.X : width >> 1;
                    int childTop = point.Y != int.MaxValue ? point.Y : height >> 1;
                    int childRight = childLeft + cw;
                    int childBottom = childTop + childHeight;
                    int x_padding = PaddingLeft - PaddingRight;
                    int y_padding = PaddingTop - PaddingRight;


                    int count = 0;
                    while ((alignment != 0) && (count++ < 3))
                    {
                        if ((alignment & 0x20) == 32)
                        {
                            childBottom = point.Y != int.MaxValue ? childTop : height;
                            childTop = childBottom - childHeight;
                            alignment ^= 0x20;
                        }
                        else if ((alignment & 0x10) == 16)
                        {
                            childTop = point.Y != int.MaxValue ? childTop : 0;
                            childBottom = childTop + childHeight;
                            alignment ^= 0x10;
                        }
                        else if ((alignment & 0x8) == 8)
                        {
                            childRight = point.X != int.MaxValue ? point.X : width;
                            childLeft = childRight - cw;
                            alignment ^= 0x8;
                        }
                        else if ((alignment & 0x4) == 4)
                        {
                            childLeft = point.X != int.MaxValue ? point.X : 0;
                            childBottom = childLeft + cw;
                            alignment ^= 0x4;
                        }
                        else if ((alignment & 0x1) == 1)
                        {
                            childLeft -= (cw >> 1);
                            childRight = childLeft + cw;
                            alignment ^= 0x1;
                        }
                        else if ((alignment & 0x2) == 2)
                        {
                            childTop -= (childHeight >> 1);
                            childBottom = childTop + childHeight;
                            alignment ^= 0x2;
                        }
                    }
                    view.Layout(childLeft + x_padding, childTop + y_padding, childRight + x_padding, childBottom + y_padding);
                }
            }
        }

        protected IProjection Projection
        {
            get { return _rotatableProjection; }
        }
    }
}