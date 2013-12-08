using System;
using System.Collections;
using System.Collections.Generic;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Java.Lang;

namespace MapQuest.Android.Maps
{
    public class LineStyle : Style
    {
        public Paint LinePaint { get; set; }
        public bool ShowPoints { get; set; }
        public Paint PointPaint { get; set; }

        private bool _shouldSimplify;
        public bool ShouldSimplify {
            get { return _shouldSimplify; }
            set { 
                throw new NotSupportedException();
            }
        }

        private readonly Path _path;
        private HandlerThread _simplifierThread = null;
        private LineOverlay.SimplifierHandler simplifierHandler = null;
        private SimplifierHandler _simplifierHandler;
        private List<List<GeoPoint>> _simplfied;
        private int _simplificationEpsilon = 9;

        public LineStyle()
        {
            _path = new Path();
        }

        public override void DrawCore(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas,
                                      DrawContext drawContext, bool shadow)
        {
#if DEBUG
            Log.Debug("com.mapquest.android.maps.lineoverlay", "LineOverlay.draw()");
#endif

            ////if (this.listener == null)
            ////{
            ////    this.listener = new EventListener(null);
            ////    mapView.addMapViewEventListener(this.listener);
            ////}
            
            Rect bounds = canvas.ClipBounds;
            Rect imageRegion = Util.CreateRectFromBoundingBox(boundingBox, drawContext.MapView);

            int pad = (int) LinePaint.StrokeWidth/2;
            imageRegion.Inset(-pad, -pad);

            BoundingBox screenBox = Util.CreateBoundingBoxFromRect(bounds, drawContext.MapView);

            foreach (var feature in features)
            {
                if (CanDraw(feature))
                {
                    DrawLineCore(feature.Geometry, GetPaint(feature), imageRegion, bounds, screenBox,
                                 drawContext.MapView, canvas);
                }
            }

        }

        protected Paint GetPaint(Feature feature)
        {
            return LinePaint;
        }

        protected virtual void DrawLineCore(IGeometry line, Paint linePaint, Rect imageRegion, Rect clipBounds,
                                            BoundingBox screenBox, MapView mapView, Canvas canvas)
        {
            var projection = mapView.Projection;
            List<GeoPoint> data = Simplify(mapView, ((LineString) line).Vertices);
            if (Rect.Intersects(imageRegion, clipBounds))
            {
                //long start = System.CurrentTimeMillis();

                _path.Reset();
                Point currentPoint = null;
                Point nextPoint = null;
                for (int i = 0; i < data.Count; i++)
                {
                    var currentGeoPoint = data[i];
                    if (!screenBox.Contains(currentGeoPoint))
                    {
                        if (i + 1 < data.Count)
                        {
                            currentPoint = projection.ToPixels(currentGeoPoint, currentPoint);
                            var nextGeoPoint = data[i + 1];
                            if (screenBox.Contains(nextGeoPoint))
                            {
                                nextPoint = projection.ToPixels(nextGeoPoint, null);
                                DrawLine(nextPoint, currentPoint);
                            }
                        }
                    }
                    else
                    {
                        currentPoint = projection.ToPixels(currentGeoPoint, currentPoint);
                        if (ShowPoints)
                        {
                            if (PointPaint == null)
                            {
                                PointPaint = CreatePointPaint();
                            }
                            canvas.DrawCircle(currentPoint.X, currentPoint.Y, PointPaint.StrokeWidth, PointPaint);
                        }
                        GeoPoint nextGeoPoint = null;
                        if (data.Count > i + 1)
                        {
                            nextGeoPoint = (GeoPoint) data[i + 1];
                        }
                        if (nextGeoPoint != null)
                        {
                            nextPoint = projection.ToPixels(nextGeoPoint, null);
                            DrawLine(currentPoint, nextPoint);
                        }
                    }
                }
                if (_path.IsEmpty)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        GeoPoint currentGeoPoint = data[i];

                        GeoPoint nextGeoPoint = null;
                        if (data.Count > i + 1)
                        {
                            nextGeoPoint = data[i + 1];
                        }
                        if (nextGeoPoint != null)
                        {
                            currentPoint = projection.ToPixels(currentGeoPoint, null);
                            nextPoint = projection.ToPixels(nextGeoPoint, null);
                            DrawLine(currentPoint, nextPoint);
                        }
                    }
                }
                
                //long end = System.currentTimeMillis();
#if DEBUG
                //Log.Debug("com.mapquest.android.maps.lineoverlay", "Time to process shapepoints: " + (float)(end - start) / 1000.0F + "; no. of points: " + data.Count);
#endif
                canvas.DrawPath(_path, LinePaint);
#if DEBUG
                //Log.Debug("com.mapquest.android.maps.lineoverlay", "Time to draw path shapepoints: " + (float)(System.currentTimeMillis() - end) / 1000.0F + "; no. of points: " + data.Count);
#endif
            }

        }

        private Paint CreatePointPaint()
        {
            if (PointPaint == null)
            {
                var paint = new Paint(PaintFlags.AntiAlias);
                paint.Color = LinePaint.Color;
                paint.Alpha = LinePaint.Alpha;
                paint.StrokeWidth = LinePaint.StrokeWidth;
                return paint;
            }

            return PointPaint;
        }

        private void DrawLine(Point from, Point to)
        {
            _path.MoveTo(from.X, from.Y);
            _path.LineTo(to.X, to.Y);
        }



        private List<GeoPoint> Simplify(MapView mapView, List<GeoPoint> data)
        {
            if (ShouldSimplify)
            {
                if (_simplifierHandler == null || _simplifierThread == null)
                {
                    _simplifierThread = new HandlerThread("simplifier", 1);
                    _simplifierThread.Start();
                    _simplifierHandler = new SimplifierHandler(mapView, _simplifierThread.Looper, new List<Point>(), data, _simplificationEpsilon);
                }
                if (_simplfied == null)
                {
                    _simplfied = new List<List<GeoPoint>>();
                    mapView.Post(new Simplifier(mapView.Projection, null, null, _simplifierHandler));
                }
                //else if (_simplified.Count != 0)
                //{
                //    data = this.simplified;
                //}
            }
            return data;
        }



        protected override bool CanDraw(Feature feature)
        {
            return feature.Geometry.GeometryType == GeometryType.LineString;
        }
    }

    public class Simplifier : Java.Lang.Object, IRunnable
    {
        private IProjection projection;
        private readonly IList<Point> _points;
        private readonly IList<GeoPoint> _data;
        private readonly SimplifierHandler _simplifierHandler;

        public Simplifier(IProjection projection, IList<Point> points, IList<GeoPoint> data, SimplifierHandler simplifierHandler)
        {
            this.projection = projection;
            _points = points;
            _data = data;
            _simplifierHandler = simplifierHandler;
        }

        public void Run()
        {
            //if (LineOverlay.this.simplify)
            //{

            int size = _data.Count;
            //LineOverlay.this.points.ensureCapacity(size);
            int p_size = _points.Count; // LineOverlay.this.points.Count;
            if (p_size < size)
            {
                while (p_size++ < size)
                {
                    _points.Add(new Point());
                }
            }
            for (int i = 0; i < size; i++)
            {
                var p = _points[i]; // (Point)LineOverlay.this.points[i];
                projection.ToPixels(_data[i], p);
            }
            _simplifierHandler.SendEmptyMessage(0);
            //}
        }
    }
}