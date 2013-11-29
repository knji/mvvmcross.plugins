using System.Collections.Generic;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class CompassOrientationFeature : Feature
    {
        public CompassOrientationFeature(Geometry geometry)
            : base(geometry, new Dictionary<string, string>())
        {
            Attributes.Add("type", typeof(CompassOrientationFeature).Name);
        }
    }
}