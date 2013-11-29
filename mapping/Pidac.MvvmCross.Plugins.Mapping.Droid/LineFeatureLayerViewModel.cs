using System.Collections.Generic;
using System.Collections.Specialized;
using MapQuest.Android.Maps;
using System.Linq;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class LineFeatureLayerViewModel : MapQuestOverlayViewModel<LineOverlay, IGeoDataManager>
    {
        public LineFeatureLayerViewModel(string name, string alias, LineOverlay layer, IGeoDataManager dataManager) : base(name, alias, layer, dataManager)
        {
        }

        protected override void RefreshCore(IEnumerable<Feature> features)
        {
            var lineFeatures = ConvertFeatures(features);
            Layer.SetData(lineFeatures.Select(l => (GeoPoint)l.Geometry).ToArray());
        }
    }
}