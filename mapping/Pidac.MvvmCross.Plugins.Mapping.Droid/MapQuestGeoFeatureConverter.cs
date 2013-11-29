using System.Collections.Generic;
using System.Linq;
using MapQuest.Android.Maps;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MapQuestGeoFeatureConverter
    {
        public static GeoFeature ConvertFeature(Feature feature)
        {
            var geom = MapQuestGeometryConverter.Convert(feature.Geometry);
            var gf = new GeoFeature(geom, feature.Geometry.GeometryType, feature.Attributes);
            return gf;
        }

        public static  IList<GeoFeature> ConvertFeatures(IEnumerable<Feature> features)
        {
            return features.Select(ConvertFeature).ToArray();
        }

        public static GeoPoint ConvertPoint(Point point)
        {
            return MapQuestGeometryConverter.ConvertPoint(point);
        }
    }
}