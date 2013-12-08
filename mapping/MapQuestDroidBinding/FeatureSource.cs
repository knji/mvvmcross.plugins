using System.Collections;
using System.Collections.Generic;

namespace MapQuest.Android.Maps
{
    public class FeatureSource : IEnumerable<Feature>
    {
        private object _lock = new object();
        private readonly GeoCollection<Feature> _features = new GeoCollection<Feature>();

        public void AddFeatures(IEnumerable<Feature> features)
        {
            lock (_lock)
            {
                _features.AddRange(features);    
            }
        }

        public IEnumerator<Feature> GetEnumerator()
        {
            return _features.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void DeleteAll()
        {
            _features.Clear();
        }

        public BoundingBox GetBoundingBox()
        {
           return BoundingBox.FromFeatures(_features);
        }
    }
}