using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MapQuest.Android.Maps
{
    public class PointStyle : Style
    {
        public PointStyle()
        {
            Color = Color.DarkRed;
        }

        public PointStyle(Drawable drawable)
        {
            Drawable = drawable;
        }

        public Color Color { get; set; }
        public Drawable Drawable { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }

        public virtual Drawable GetMarker(Feature feature, AssetManager assetManager)
        {
            SetState(Drawable, 0);
            return Drawable;
        }

        public override void DrawCore(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow)
        {
            var projection = drawContext.MapView.Projection;
            var bounds = canvas.ClipBounds;

            BoundingBox screenBox = Util.CreateBoundingBoxFromRect(bounds, drawContext.MapView);
            if (BoundingBox.Intersect(boundingBox, screenBox))
            {
                Point point = new Point();
                foreach (var feature in features)
                {
                    // there is a condition that checks for focus index before drawing
                    if (!CanDraw(feature))
                        continue;

                    Drawable marker = GetMarker(feature, drawContext.Assets);
                    //if (item.Alignment != 0)
                    //{
                    //    Overlay.SetAlignment(marker, item.Alignment);
                    //}
                    var geoPoint = feature.Geometry as GeoPoint;
                    if (geoPoint == null) continue;

                    projection.ToPixels(geoPoint, point);
                    DrawFeature(canvas, feature, point, marker, shadow);
                }

                //int size = this.items.size();
                //for (int i = size - 1; i >= 0; i--)
                //{
                //    if (this.focusedIndex != i)
                //    {
                //        OverlayItem item = getItem(i);

                //        item.
                //        Drawable marker = GetMarker(item);
                //        if (item.Alignment != 0)
                //        {
                //            Overlay.SetAlignment(marker, item.Alignment);
                //        }
                //        projection.ToPixels(item.Point, point);
                //        DrawFeature(canvas, item, point, marker, shadow);
                //    }
                //}

                //if ((this.drawFocusedItem) && (this.focusedIndex != -1))
                //{
                //    Item item = getItem(this.focusedIndex);
                //    projection.ToPixels(item.getPoint(), point);
                //    drawItem(canvas, getItem(this.focusedIndex), point, getMarker(item), shadow);
                //}
            }
        }

        protected override bool CanDraw(Feature feature)
        {
            return feature.Geometry.GeometryType == GeometryType.Point;
        }

        public static void SetState(Drawable marker, int stateBitset)
        {
            int[] states = new int[3];

            int index = 0;
            if ((stateBitset & 0x2) > 0) {
                states[index++] = 16842919;
            }
            if ((stateBitset & 0x1) > 0) {
                states[index++] = 16842913;
            }
            if ((stateBitset & 0x4) > 0) {
                states[index] = 16842908;
            }
            marker.SetState(states);
        }

        void DrawFeature(Canvas canvas, Feature item, Point point, Drawable marker, bool shadow)
        {
            Bounds.Set(marker.Bounds);
            Bounds.Offset(point.X, point.Y);

            Rect bounds = canvas.ClipBounds;
            if (Rect.Intersects(bounds, bounds))
            {
                DrawAt(canvas, marker, point.X, point.Y, shadow);
            }
        }
    }
}