using System.Collections.Generic;
using System.Linq;
using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class PolygonFeatureLayerViewModel : MapQuestOverlayViewModel<PolygonOverlay, IGeoDataManager>
    {
        public PolygonFeatureLayerViewModel(string name, string alias, PolygonOverlay layer, IGeoDataManager dataManager) : base(name, alias, layer, dataManager)
        {
        }

        protected override void RefreshCore(IEnumerable<Feature> features)
        {
            var lineFeatures = ConvertFeatures(features);
            Layer.SetData(lineFeatures.Select(l => (GeoPoint)l.Geometry).ToArray());
        }
    }
}