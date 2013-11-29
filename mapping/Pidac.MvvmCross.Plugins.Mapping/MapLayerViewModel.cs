using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public abstract class MapLayerViewModel<TLayer, TDataManager> : ILayerViewModel
        where TLayer : IDisposable
        where TDataManager : class, IGeoDataManager
    {
        protected TLayer Layer { get; set; }
        public event EventHandler LayerRefreshed;

        public TDataManager DataManager { get; private set; }

        public int GetFeaturesCount()
        {
            return DataManager.GetFeaturesCount();
        }

        public string Alias { get; set; }
        public virtual string Name { get; set; }

        protected MapLayerViewModel(TLayer layer, string name, string alias, TDataManager dataManager)
        {
            Layer = layer;
            DataManager = dataManager;

// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Name = name;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
            Alias = alias;
            Initialize();
        }

        private void Initialize()
        {
            if (DataManager == null)
                throw new ArgumentException("data manager cannot be null");

            if (DataManager.IsDynamic)
            {
                DataManager.CollectionChanged += OnGeoDataManagerCollectionChanged;
            }
        }

        protected abstract void OnGeoDataManagerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

        public BoundingBox GetBoundingBox()
        {
            return DataManager.GetBounds();
        }

        protected abstract void RefreshCore(IEnumerable<Feature> features);

        public void CompleteInit()
        {
            var features = DataManager.GetFeatures();
            Refresh(features);
        }

        public object GetLayer()
        {
            return Layer;
        }

        public void Dispose()
        {
            Layer.Dispose();
            if (DataManager != null)
            {
                DataManager.CollectionChanged -= OnGeoDataManagerCollectionChanged;
            }
        }

        public void Refresh(IEnumerable<Feature> features)
        {
            OnLayerRefreshing();
            RefreshCore(features);
            OnLayerRefreshed();
        }

        protected virtual void OnLayerRefreshed()
        {
            if (LayerRefreshed != null)
            {
                LayerRefreshed(this, EventArgs.Empty);
            }
        }

        protected void OnLayerRefreshing()
        {
            // riase events
        }
    }
}