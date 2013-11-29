using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class FeatureCollection : ObservableCollection<Feature>
    {
        public FeatureCollection()
        {
        }

        public FeatureCollection(IEnumerable<Feature> features) : base(features)
        {
        }

        public BoundingBox GetBounds()
        {
            if (Count == 0) return new BoundingBox(); // or throw?
            return new BoundingBox(this.Select(f => f.Geometry).ToArray());
        }
    }
}