using System.Collections.Generic;
using System.Collections.Specialized;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class GeoDataManager : IGeoDataManager
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected readonly FeatureCollection Features = new FeatureCollection();

        public GeoDataManager() : this(null)
        {
        }

        public GeoDataManager(IEnumerable<Feature> features )
        {
            Capacity = 100;

            if (features != null)
            {
                foreach (var feature in features)
                {
                    Features.Add(feature);
                }
            }

            Features.CollectionChanged += (sender, args) =>
            {
                if (CollectionChanged != null)
                    CollectionChanged(this, args);
            };

        }

        public IEnumerable<Feature> GetFeatures()
        {
            return Features;
        }
      
        public int GetFeaturesCount()
        {
            return Features.Count;
        }

        public int Capacity { get; set; }
        public bool IsDynamic { get; protected set; }

        private GeometryType _geometryType;
        public GeometryType GeometryType
        {
            get
            {
                if (Features.Count > 0)
                    return Features[0].Geometry.GeometryType;
                return _geometryType;
            }

            protected set { _geometryType = value; }
        }

        public void AddFeature(Feature feature)
        {
            if (Features.Count > Capacity)
            {
                // todo consider bulk remove..
                Features.RemoveAt(0);
            }

            Features.Add(feature);
        }

        public BoundingBox GetBounds()
        {
            return Features.GetBounds();
        }
    }
}