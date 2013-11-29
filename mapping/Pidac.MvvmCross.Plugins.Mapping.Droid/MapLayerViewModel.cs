using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
//    public abstract class MapLayerViewModel<T> : ILayerViewModel where T : IDisposable
//    {
//        private readonly MapView _mapView;
//        public IGeoDataManager DataManager { get; private set; }
//        public T Layer { get; set; }
//        public string Alias { get; set; }
//        public virtual string Name { get; set; }

//        protected MapLayerViewModel(T layer, string name, string alias, IGeoDataManager dataManager, MapView mapView)
//        {
//            _mapView = mapView;

//            Layer = layer;
//            DataManager = dataManager;

//// ReSharper disable DoNotCallOverridableMethodsInConstructor
//            Name = name;
//// ReSharper restore DoNotCallOverridableMethodsInConstructor
//            Alias = alias;
//            Initialize();
//        }

//        private void Initialize()
//        {
//            if (DataManager == null)
//                throw new ArgumentException("data manager cannot be null");

//            if (DataManager.IsDynamic)
//            {
//                DataManager.CollectionChanged += OnGeoDataManagerCollectionChanged;
//            }

//            var features = DataManager.GetFeatures();
//            Refresh(features);
//        }

//        protected virtual void OnGeoDataManagerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            Refresh(DataManager.GetFeatures());
//        }

//        public object GetLayer()
//        {
//            return Layer;
//        }

//        public void Dispose()
//        {
//            Layer.Dispose();
//            if (DataManager != null)
//            {
//                DataManager.CollectionChanged -= OnGeoDataManagerCollectionChanged;
//            }
//        }

//        public void Refresh(IEnumerable<Feature> features)
//        {
//            RefreshCore(features);
//            OnLayerRefreshed();
//        }

//        protected abstract void RefreshCore(IEnumerable<Feature> features);

//        protected virtual void OnLayerRefreshed()
//        {
//            var bounds = GetLayerBounds();
//            _mapView.Invalidate();
//            _mapView.Controller.ZoomToSpan(bounds);   //AnimateTo(currentLocation);   // what bounding box?
//            //_mapView.Controller.SetZoom(14);  // what zoom level?
            
//        }

//        private BoundingBox GetLayerBounds()
//        {
//            var bbox = DataManager.GetBounds(); 
//            // use static instead of creating new object for performance?
//            return new MapQuestGeometryConverter().ConvertBoundingBox(bbox);
//        }
//    }
}