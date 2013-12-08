using System;
using System.Collections.Generic;
using System.Linq;
using MapQuest.Android.Maps;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;
using BoundingBox = Pidac.MvvmCross.Plugins.Mapping.Geometries.BoundingBox;
using Geometry = Pidac.MvvmCross.Plugins.Mapping.Geometries.Geometry;
using GeometryType = Pidac.MvvmCross.Plugins.Mapping.Geometries.GeometryType;
using LineString = Pidac.MvvmCross.Plugins.Mapping.Geometries.LineString;


namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public static class MapQuestGeometryConverter 
    {
        public static object Convert(Geometry geometry)
        {
            if (geometry.GeometryType == GeometryType.Point)
                return ConvertPoint((Point) geometry);
            if (geometry.GeometryType == GeometryType.LineString)
                return ConvertLine((LineString) geometry);
            if (geometry.GeometryType == GeometryType.LinearRing)
                return ConvertLine((LinearRing) geometry);
            if (geometry.GeometryType == GeometryType.Polygon)
                return ConvertPolygon((Polygon) geometry);

            throw new ArgumentException(string.Format("{0} geometry type not supported.", geometry.GeometryType));
        }

        public static MapQuest.Android.Maps.BoundingBox ConvertBoundingBox(BoundingBox bbox)
        {
            return new MapQuest.Android.Maps.BoundingBox(new GeoPoint(bbox.NorthEast.Y, bbox.SouthWest.X), new GeoPoint(bbox.SouthWest.Y, bbox.NorthEast.X));
        }

        public static GeoPoint ConvertPoint(Point point)
        {
            return new GeoPoint(point.Y, point.X);
        }

        public static MapQuest.Android.Maps.LineString ConvertLine(LineString line)
        {
            return new MapQuest.Android.Maps.LineString(line.Vertices.Select(ConvertPoint).ToArray());
        }

        public static IList<GeoPoint> ConvertPolygon(Polygon polygon)
        {
            //if (polygon.Rings.Count > 1)
            //    throw new ArgumentException("polygon with multiple rings not currently supported. ");
            //return ConvertLine(polygon.Rings.First());

            throw new NotImplementedException();
        }
    }
}