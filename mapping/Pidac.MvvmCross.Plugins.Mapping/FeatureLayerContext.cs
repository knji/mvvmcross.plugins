using Pidac.MvvmCross.Plugins.Mapping.Geometries;
using Pidac.MvvmCross.Plugins.Mapping.Styling;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class FeatureLayerContext 
    {
        public GeometryType GeometryType
        {
            get { return GeoDataManager.GeometryType; }
        }
        public StyleInfo StyleInfo
        {
            get { return LayerDescriptor != null ? LayerDescriptor.StyleInfo : null; }
        }

        public StyleType StyleType
        {
            get { return StyleInfo != null ? StyleInfo.StyleType : StyleType.NotDefined; }
        }

        public FeatureLayerDescriptor LayerDescriptor { get; set; }
        public IGeoDataManager GeoDataManager { get; set; }

        public string Name
        {
            get { return LayerDescriptor != null ? LayerDescriptor.Name : null; }
        }

        public string Alias
        {
            get { return LayerDescriptor != null ? LayerDescriptor.Alias : null; }
        }
    }
}