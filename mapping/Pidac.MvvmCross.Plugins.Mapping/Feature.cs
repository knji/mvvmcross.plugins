using System;
using System.Collections.Generic;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;
   
namespace Pidac.MvvmCross.Plugins.Mapping
{
    /// <summary>
    /// Feature class modeled after OpenLayers
    /// http://dev.openlayers.org/releases/OpenLayers-2.11/doc/apidocs/files/OpenLayers/Feature/Vector-js.html
    /// </summary>
    public class Feature
    {
        public const string TypeKey = "type";

        /// <summary>
        /// Feature style in SLD?
        /// </summary>
        public object Style { get; set; }
      
        /// <summary>
        /// Feature attributes
        /// </summary>
        public IDictionary<string, string> Attributes { get; private set; }


        /// <summary>
        /// Feature geometry in OGC WKT
        /// </summary>
        public Geometry Geometry { get; private set; }
        public DateTime TimeStamp { get; set; }

        public Feature(Geometry geometry, IDictionary<string, string> attributes = null )
        {
            Geometry = geometry;
            Attributes = attributes ?? new Dictionary<string, string>();
            TimeStamp = DateTime.UtcNow;
        }

        public string GetFeatureType()
        {
            if (Attributes != null && Attributes.ContainsKey(TypeKey))
                return Attributes["type"];

            return null;
        }
    }
}