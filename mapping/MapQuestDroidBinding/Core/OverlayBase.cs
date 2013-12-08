using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

namespace MapQuest.Android.Maps.Core
{
    public interface IOverlayTrackballEventListener
    {
        void OnTrackballEvent(MotionEvent paramMotionEvent, MapView paramMapView);
    }

    public interface IOverlayTouchEventListener
    {
        void OnTouch(MotionEvent paramMotionEvent, MapView paramMapView);
    }

    public interface IOverlayTapListener
    {
        void OnTap(GeoPoint paramGeoPoint, MapView paramMapView);
    }

    public interface ISnappable
    {
        bool OnSnapToItem(int paramInt1, int paramInt2, Point paramPoint, MapView paramMapView);
    }

    public abstract class Overlay
    {
        protected IOverlayTapListener TapListener;
        protected IOverlayTouchEventListener TouchListener;
        protected IOverlayTrackballEventListener TrackballListener;
        private String _key;
        private int _zIndex;
        protected static float SHADOW_X_SKEW = -0.9F;
        protected static float SHADOW_Y_SCALE = 0.5F;
        public static int CENTER_HORIZONTAL = 1;
        public static int CENTER_VERTICAL = 2;
        public static int CENTER = 3;
        public static int LEFT = 4;
        public static int RIGHT = 8;
        public static int TOP = 16;
        public static int BOTTOM = 32;

        protected Overlay()
        {
            _key = "";
            _zIndex = 0;
        }

        public static Drawable SetAlignment(Drawable drawable, int bitset)
        {
            if (drawable != null)
            {
                int w = drawable.IntrinsicWidth;
                int h = drawable.IntrinsicHeight;

                int l = -(w >> 1);
                int r = l + w;
                int t = -(h >> 1);
                int b = t + h;
                int count = 0;
                while ((bitset != 0) && (count++ < 3))
                {
                    if ((bitset & 0x1) > 0)
                    {
                        l = -(w >> 1);
                        r = l + w;
                        bitset ^= 0x1;
                    }
                    else if ((bitset & 0x2) > 0)
                    {
                        t = -(h >> 1);
                        b = t + h;
                        bitset ^= 0x2;
                    }
                    else if ((bitset & 0x20) > 0)
                    {
                        t = -h;
                        b = 0;
                        bitset ^= 0x20;
                    }
                    else if ((bitset & 0x10) > 0)
                    {
                        t = 0;
                        b = h;
                        bitset ^= 0x10;
                    }
                    else if ((bitset & 0x8) > 0)
                    {
                        r = 0;
                        l = -w;
                        bitset ^= 0x8;
                    }
                    else if ((bitset & 0x4) > 0)
                    {
                        l = 0;
                        r = w;
                        bitset ^= 0x4;
                    }
                }
                drawable.SetBounds(l, t, r, b);
            }
            return drawable;
        }

        protected static void DrawAt(Canvas canvas, Drawable drawable, int x, int y, bool shadow)
        {
            try
            {
                canvas.Save();
                canvas.Translate(x, y);
                if (shadow)
                {
                    drawable.SetColorFilter(Util.Int32ToColor(2130706432), PorterDuff.Mode.SrcIn);
                    canvas.Skew(-0.9F, 0.0F);
                    canvas.Scale(1.0F, 0.5F);
                }
                drawable.Draw(canvas);
                if (shadow)
                {
                    drawable.ClearColorFilter();
                }
            }
            finally
            {
                canvas.Restore();
            }
        }

        public void SetTapListener(IOverlayTapListener overlayTapListener)
        {
            TapListener = overlayTapListener;
        }

        public void SetTouchEventListener(IOverlayTouchEventListener touchListener)
        {
            TouchListener = touchListener;
        }

        public void SetTrackballEventListener(IOverlayTrackballEventListener trackballListener)
        {
            TrackballListener = trackballListener;
        }

        public abstract void Draw(Canvas canvas, MapView mapView, bool shadow);

        public bool Draw(Canvas canvas, MapView mapView, bool shadow, long when)
        {
            Draw(canvas, mapView, shadow);
            return false;
        }

        public bool OnKeyDown(int keyCode, KeyEvent keyEvent, MapView mapView)
        {
            return false;
        }

        public bool OnKeyUp(int keyCode, KeyEvent keyEvent, MapView mapView)
        {
            return false;
        }

        public bool OnTap(GeoPoint p, MapView mapView)
        {
            return false;
        }

        public bool OnTouchEvent(MotionEvent evt, MapView mapView)
        {
            return false;
        }

        public bool OnTrackballEvent(MotionEvent evt, MapView mapView)
        {
            return false;
        }

        public virtual void Destroy() { }

        public int GetZIndex()
        {
            return _zIndex;
        }

        public void SetZIndex(int zIndex)
        {
            _zIndex = zIndex;
        }

        public String GetKey()
        {
            return _key;
        }

        public void SetKey(String key)
        {
            _key = key;
        }


    }


}