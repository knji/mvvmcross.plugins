using System;
using System.Collections.Generic;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public interface ILayerViewModel
    {
        event EventHandler LayerRefreshed;
        int GetFeaturesCount();
        string Alias { get; }
        string Name { get; }
        object GetLayer();
        void Dispose();
        void Refresh(IEnumerable<Feature> features);
        void CompleteInit();
        BoundingBox GetBoundingBox();

    }
}