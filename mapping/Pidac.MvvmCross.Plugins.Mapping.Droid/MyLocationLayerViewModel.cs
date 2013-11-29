using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MyLocationLayerViewModel : MapQuestOverlayViewModel<MyLocationOverlay, IUserLocationTracker>
    {
        private readonly MapView _mapView;

        public MyLocationLayerViewModel(string name, string alias, MyLocationOverlay layer,
            IUserLocationTracker dataManager, MapView mapView) : 
            base(name, alias, layer, dataManager)
        {
            _mapView = mapView;
            //layer.UserLocationTracker = dataManager;
        }

        protected override void RefreshCore(IEnumerable<Feature> features)
        {
            Layer.Clear();
            Layer.AddItem(CreateCurrentCompassFeature());
            Layer.AddItem(CreateCurrentLocationFeature());
        }

        protected override void OnLayerRefreshed()
        {
            // 
            //_mapView.Controller.AnimateTo(MapQuestGeoFeatureConverter.ConvertPoint(DataManager.GetLatestLocation()));
            //_mapView.Controller.SetZoom(14);  // what level should we use?
        }

        private OverlayItem CreateCurrentLocationFeature()
        {
            var feature = DataManager.GetLatestLocationFeature();
            var mqfeature = MapQuestGeoFeatureConverter.ConvertFeature(feature);
            return new OverlayItem((GeoPoint)mqfeature.Geometry, feature.Attributes[Feature.TypeKey], null);
        }

        public OverlayItem CreateCurrentCompassFeature()
        {
            var feature = DataManager.GetLatestCompassFeature();
            var mqfeature = MapQuestGeoFeatureConverter.ConvertFeature(feature);
            return new OverlayItem((GeoPoint)mqfeature.Geometry, feature.Attributes[Feature.TypeKey], null);
        }
    }
}