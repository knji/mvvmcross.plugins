using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class PointFeatureLayerViewModel : MapQuestOverlayViewModel<DefaultItemizedOverlay, IGeoDataManager>
    {
       private readonly StringBuilder _stringBuilder = new StringBuilder();
        public PointFeatureLayerViewModel(string name, string alias, DefaultItemizedOverlay layer, IGeoDataManager dataManager)
            : base(name, alias, layer, dataManager)
        {
        }

        protected override void RefreshCore(IEnumerable<Feature> features)
        {
            var geoFeatures = ConvertFeatures(features);
            Layer.Clear();
            foreach (var feature in geoFeatures)
            {
                Layer.AddItem(CreateOverlayItem(feature));
            }
        }

        public OverlayItem CreateOverlayItem(GeoFeature feature)
        {
            string field1 = null;

            if (feature.Fields != null)
            {
                if (feature.Fields.Keys.Count > 1)
                {
                    // consider string builder
                    foreach (var field in feature.Fields)
                    {
                        _stringBuilder.Append(field.Value + ",");
                    }

                    _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
                    field1 = _stringBuilder.ToString();
                    _stringBuilder.Clear();
                }
                else
                {
                    field1 = feature.Fields.Values.First();
                }
            }

            return new OverlayItem((GeoPoint) feature.Geometry, field1, null);
        }
    }


}