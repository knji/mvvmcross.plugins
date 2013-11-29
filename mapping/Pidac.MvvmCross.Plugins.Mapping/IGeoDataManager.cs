using System.Collections.Generic;
using System.Collections.Specialized;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public interface IGeoDataManager : INotifyCollectionChanged
    {
        int GetFeaturesCount();
        int Capacity { get; set; }
        GeometryType GeometryType { get; }
        IEnumerable<Feature> GetFeatures();
        bool IsDynamic { get; }
        BoundingBox GetBounds();
    }
}