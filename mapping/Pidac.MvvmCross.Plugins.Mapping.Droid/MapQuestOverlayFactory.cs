using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using MapQuest.Android.Maps;
using Pidac.MvvmCross.Plugins.Mapping;
using Pidac.MvvmCross.Plugins.Mapping.Droid;
using Pidac.MvvmCross.Plugins.Mapping.Styling;
using MyLocationOverlay = MapQuest.Android.Maps.MyLocationOverlay;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MapQuestOverlayFactory
    {
        public ILayerViewModel CreateOverlay(FeatureLayerContext layerContext, 
            MapView mapView, 
            AssetManager assetManager,
            AnnotationView annotationView = null)
        {
            if (layerContext.StyleType == StyleType.Point)
            {
                var overlay = CreatePointOverlay((PointStyleInfo)layerContext.StyleInfo, assetManager, annotationView);
                return new PointFeatureLayerViewModel(layerContext.Name, layerContext.Alias, overlay, layerContext.GeoDataManager);
            }

            //if (layerContext.StyleType == StyleType.Point)
            //{
            //    var style = MapquestStyleFactory.CreatePointStyle((PointStyleInfo)layerContext.StyleInfo, assetManager);
            //    return CreateFeatureLayerViewModel(style, assetManager, annotationView, layerContext);
            //}

            //if (layerContext.StyleType == StyleType.Line)
            //{
            //    var lineOverlay = CreateLineOverlay((LineStyleInfo) layerContext.StyleInfo);
            //    return new LineFeatureLayerViewModel(layerContext.Name, layerContext.Alias, lineOverlay, layerContext.GeoDataManager);
            //}

            if (layerContext.StyleType == StyleType.Line)
            {
                var lineStyle = MapquestStyleFactory.CreateLineStyle((LineStyleInfo)layerContext.StyleInfo);
                return CreateFeatureLayerViewModel(lineStyle, assetManager, annotationView, layerContext);
            }

            if (layerContext.StyleType == StyleType.Polygon)
            {
                var overlay = CreatePolygonOverlay((PolygonStyleInfo)layerContext.StyleInfo);
                return new PolygonFeatureLayerViewModel(layerContext.Name, layerContext.Alias, overlay, layerContext.GeoDataManager);
            }

            throw new NotSupportedException(string.Format("layer content style {0} not supported.", layerContext.StyleType));
        }

        public FeatureLayerViewModel CreateFeatureLayerViewModel(Style style, AssetManager assetManager,
                                                                 AnnotationView annotationView, FeatureLayerContext layerContext)
        {
            var overlay = CreateVectorOverlay(style, assetManager, annotationView);
            return new FeatureLayerViewModel(layerContext.Name, layerContext.Alias, overlay, layerContext.GeoDataManager);
        }

        public VectorOverlay CreateVectorOverlay(Style style, AssetManager assetManager, AnnotationView annotationView = null)
        {
//            var drawable = CreateDrawable(styleInfo, assetManager);
            var overlay = new VectorOverlay();
            overlay.AddCustomStyle(style);
            return overlay;
        }

        public DefaultItemizedOverlay CreatePointOverlay(PointStyleInfo styleInfo, AssetManager assetManager, AnnotationView annotationView = null)
        {
            var drawable = CreateDrawable(styleInfo, assetManager) ; 
            var overlay = new DefaultItemizedOverlay(drawable);
            overlay.SetTapListener(new TapOverlayListener(overlay, annotationView));
            return overlay;
        }

        private Drawable CreateDrawable(PointStyleInfo styleInfo, AssetManager assetManager )
        {
            if (string.IsNullOrWhiteSpace(styleInfo.ImageUrl))
            {
                throw new NotSupportedException("point style for non-images not supported.");
            }

            var asset = assetManager.Open(styleInfo.ImageUrl);
            return new BitmapDrawable(asset);
        }

        public Color CreateColor(Colour color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
     
      

        public LineOverlay CreateLineOverlay( LineStyleInfo styleInfo)
        {
            var paint = MapquestStyleFactory.CreatePaintFromStyleInfo(styleInfo);
            var lineOverlay = new LineOverlay(paint);

            // get from styleInfo
            var paint1 = new Paint(PaintFlags.AntiAlias);
            paint1.StrokeWidth = styleInfo.StrokeWidth;
            paint1.Alpha = 100;
            paint1.Color = new Color(0, 0, 0, 255);
            lineOverlay.SetShowPoints(true, paint1);
            return lineOverlay;
        }

        public PolygonOverlay CreatePolygonOverlay(PolygonStyleInfo styleInfo)
        {
            //var paint = new Paint(PaintFlags.AntiAlias);
            //paint.Color = Color.Black;
            //paint.SetStyle(Paint.Style.Stroke);
            //paint.StrokeWidth = 3;
            //paint.Alpha = 100;
            //paint.StrokeJoin = Paint.Join.Round;
            //paint.StrokeCap = Paint.Cap.Round;

            var paint = MapquestStyleFactory.CreatePaintFromStyleInfo(styleInfo);
            var polygonOverlay = new PolygonOverlay(paint);
            return polygonOverlay;
        }

        public MyLocationLayerViewModel CreateMyLocationOverlay(string name, string alias,
            PointStyleInfo userSymbol, 
            PointStyleInfo compassSymbol, 
            AssetManager assetManager,
            IUserLocationTracker userLocationDataManager,
            MapView mapVIew,
            AnnotationView annotationView = null)
        {
            var userDrawable = CreateDrawable(userSymbol, assetManager);
            Drawable compassDrawable = null;
            if (compassSymbol != null)
            {
                compassDrawable = CreateDrawable(compassSymbol, assetManager);
            }
            var layer = new MyLocationOverlay(userDrawable, compassDrawable);
            layer.SetTapListener(new TapOverlayListener(layer, annotationView));
            return new MyLocationLayerViewModel(name, alias, layer, userLocationDataManager, mapVIew);
        }
    }
}