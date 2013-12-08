using System;
using System.Collections.Generic;
using System.Linq;

namespace MapQuest.Android.Maps
{
    public partial class BoundingBox
    {
        public static readonly BoundingBox Empty = new BoundingBox { IsEmpty = true };
        public bool IsEmpty { get; private set; }

        public BoundingBox Union(IGeometry geometry)
        {
            var bbox = geometry.GetBoundingBox();
            return Union(bbox);
        }

        public BoundingBox Union(BoundingBox bbox)
        {
            var lrX = Math.Min(Lr.Longitude, bbox.Lr.Longitude);
            var lrY = Math.Min(Lr.Latitude, bbox.Lr.Latitude);
            var ulX = Math.Max(Ul.Longitude, bbox.Ul.Longitude);
            var ulY = Math.Max(Ul.Latitude, bbox.Ul.Latitude);

            Lr = new GeoPoint(lrY, lrX);
            Ul = new GeoPoint(ulY, ulX);
            return this;
        }

        public static BoundingBox FromFeatures(IList<Feature> features)
        {
            if (features == null)
                throw new ArgumentNullException("features");

            if (features.Count == 0) return Empty;

            return FromGeometries(features.Select(f => f.Geometry).ToArray());
        }

        public static BoundingBox FromGeometries(IList<IGeometry> geoms)
        {
            if (geoms == null)
                throw new ArgumentNullException("geoms");

            if (geoms.Count == 0) return Empty;

            var bbox = geoms[0].GetBoundingBox();
            for (var i = 1; i < geoms.Count; i++)
                bbox.Union(geoms[i].GetBoundingBox());

            return bbox;
        }
    }
}