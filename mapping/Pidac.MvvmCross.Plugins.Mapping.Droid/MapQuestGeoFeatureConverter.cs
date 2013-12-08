using System.Collections.Generic;
using System.Linq;
using MapQuest.Android.Maps;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;
using IGeometry = MapQuest.Android.Maps.IGeometry;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MapQuestGeoFeatureConverter
    {
        public static MapQuest.Android.Maps.Feature Convert(Feature feature)
        {
            var geom = MapQuestGeometryConverter.Convert(feature.Geometry);
            var gf = new MapQuest.Android.Maps.Feature((IGeometry)geom, feature.Attributes);
            return gf;
        }

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