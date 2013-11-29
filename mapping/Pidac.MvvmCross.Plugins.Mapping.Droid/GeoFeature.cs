using System;
using System.Collections.Generic;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class GeoFeature
    {
        public GeoFeature(object geometry, GeometryType type, IDictionary<string, string> values = null)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry");

            Geometry = geometry;
            GeometryType = type;
            Fields = values;
        }

        public GeometryType GeometryType { get; private set; }
        public object Geometry { get; private set; }
        public IDictionary<string, string> Fields { get; set; }
    }
}