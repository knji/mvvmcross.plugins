using System.Collections.Generic;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class UserLocationFeature : Feature
    {
        public UserLocationFeature(Geometry geometry) : base(geometry, new Dictionary<string, string>())
        {
            Attributes.Add("type", typeof(UserLocationFeature).Name);
        }
    }
}