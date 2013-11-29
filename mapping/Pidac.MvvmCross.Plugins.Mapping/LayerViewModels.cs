using System.Collections.ObjectModel;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class LayerViewModels : ObservableCollection<ILayerViewModel>
    {
        public BoundingBox GetBoundingBox()
        {
            if (Items.Count == 0)
                return BoundingBox.Empty;

            var bbox = new BoundingBox(Items[0].GetBoundingBox());
            for (var i = 1; i < Items.Count; i++)
            {
                bbox = bbox.Union(Items[i].GetBoundingBox());
            }

            return bbox;
        }
    }
}