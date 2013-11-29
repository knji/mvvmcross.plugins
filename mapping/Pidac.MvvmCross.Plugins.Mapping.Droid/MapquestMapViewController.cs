using System;
using System.Collections.Generic;
using Android.Content.Res;
using Cirrious.CrossCore.Droid;
using MapQuest.Android.Maps;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;
using Pidac.MvvmCross.Plugins.Mapping.Styling;
using BoundingBox = MapQuest.Android.Maps.BoundingBox;


namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MapquestMapViewController : IMapViewController
    {
        private readonly AssetManager _assetManager;
        private IMapView _mapView;
        private MapView _map;
        private AnnotationView _annotationView;
        private readonly List<FeatureLayerContext> _pendingLayersForDraw = new List<FeatureLayerContext>();
        private readonly List<Action> _pendingLayerDrawActions = new List<Action>();
        private readonly MapQuestOverlayFactory _mapQuestOverlayFactory = new MapQuestOverlayFactory(); 

        public MapquestMapViewController(IMvxAndroidGlobals androidGlobals)
        {
            LayerViewModels = new LayerViewModels();
            _assetManager = androidGlobals.ApplicationContext.Assets;
        }

        public void SetActiveMap(IMapView view, Point center, int zoomLevel)
        {
            _mapView = view;
            if (_mapView != null)
            {
                _map = (MapView) _mapView.GetMapObject();
                SetupMap(_map, center, zoomLevel);
            }
        }

        public void AddMyLocationLayer(
            string name, string alias,
            PointStyleInfo userSymbol,
            PointStyleInfo compassSymbol = null, 
            IUserLocationTracker userLocationDataManager = null)
        {
            if (_mapView == null)
            {
                _pendingLayerDrawActions.Add(() => CreateMyLocationOverlay(name, alias, userSymbol, compassSymbol, userLocationDataManager));
            }
            else
            {
                CreateMyLocationOverlay(name, alias, userSymbol, compassSymbol, userLocationDataManager);
            }
            
            //locationOverlay.IsLocationUpdateEnabled = true;  //.EnableLocationTracking();
            //locationOverlay.RunOnFirstFix(new MyLocationRunnable(locationOverlay, _map));
        }

        private void CreateMyLocationOverlay(string name, string alias,
            PointStyleInfo userSymbol,
            PointStyleInfo compassSymbol = null,
            IUserLocationTracker userLocationDataManager = null)
        {
            var locationOverlay = 
                _mapQuestOverlayFactory.CreateMyLocationOverlay(name, alias, 
                userSymbol, compassSymbol, 
                _assetManager, userLocationDataManager, _map, _annotationView);
            RegisterLayerViewModel(locationOverlay);
        }

        public void AddFeatureLayer(FeatureLayerContext layerContext)
        {
            if (_mapView == null)
            {
                _pendingLayerDrawActions.Add(() => CreateFeatureLayerCore(layerContext));
            }
            else
            {
                CreateFeatureLayerCore(layerContext);
            }
        }

        private void CreateFeatureLayerCore(FeatureLayerContext layerContext)
        {
            var layer = _mapQuestOverlayFactory.CreateOverlay(layerContext, _map, _assetManager, _annotationView);
            RegisterLayerViewModel(layer);
        }

        private void RegisterLayerViewModel(ILayerViewModel layerViewModel)
        {
            layerViewModel.LayerRefreshed += LayerViewModelOnLayerRefreshed;
            LayerViewModels.Add(layerViewModel);
            _map.Overlays.Add((Overlay) layerViewModel.GetLayer());
            layerViewModel.CompleteInit();
        }

        private void LayerViewModelOnLayerRefreshed(object sender, EventArgs eventArgs)
        {
            var layerRawBounds = LayerViewModels.GetBoundingBox().BufferBy(0.00001F);
            if (layerRawBounds.GetArea() > 0)
            {
                var layer = (ILayerViewModel) sender;
                if (layer.GetFeaturesCount() > 1)
                {
                    var layerBounds = MapQuestGeometryConverter.ConvertBoundingBox(layerRawBounds);
                    _map.Controller.ZoomToSpan(layerBounds);
                }
                else
                {
                    var layerBounds = MapQuestGeometryConverter.ConvertBoundingBox(layer.GetBoundingBox());
                    _map.Controller.ZoomToSpan(layerBounds);
                }
            }

            _map.PostInvalidate();
        }

        public LayerViewModels LayerViewModels { get; private set; }

        public void DrawPendngLayers()
        {
            foreach (var featureLayerContext in _pendingLayersForDraw)
            {
                CreateFeatureLayerCore(featureLayerContext);
            }
            _pendingLayersForDraw.Clear();

            foreach (var action in _pendingLayerDrawActions)
            {
                action.Invoke();
            }
            _pendingLayerDrawActions.Clear();
        }

        public void DisposeActiveMapContext()
        {
            foreach (var layerViewModel in LayerViewModels)
            {
                layerViewModel.LayerRefreshed -= LayerViewModelOnLayerRefreshed;
            }
            LayerViewModels.Clear();

            if (_map != null)
            {
                _map.Destroy();
                _mapView = null;
                _map = null;
            }
        }

        private void SetupMap(MapView map, Point center, int zoomLevel)
        {
            map.Controller.SetZoom(zoomLevel);
            map.Controller.SetCenter(new GeoPoint(center.Y, center.X));
            map.SetBuiltInZoomControls(true);

            _annotationView = new AnnotationView(map);
        }


        //protected override bool IsRouteDisplayed
        //{
        //    get { return false; }
        //}


        //public class MyLocationRunnable : Object, IRunnable
        //{
        //    private readonly MyLocationOverlay _overlay;
        //    private readonly MapView _map;

        //    public MyLocationRunnable(MyLocationOverlay overlay, MapView map)
        //    {
        //        _overlay = overlay;
        //        _map = map;
        //    }

        //    public void Run()
        //    {
        //        var currentLocation = _overlay.LastLocation; 
        //        _map.Controller.AnimateTo(currentLocation);
        //        _map.Controller.SetZoom(14);
        //        _map.Overlays.Add(_overlay);
        //        _overlay.IsFollowing = true;
        //    }
        //}
    }
}