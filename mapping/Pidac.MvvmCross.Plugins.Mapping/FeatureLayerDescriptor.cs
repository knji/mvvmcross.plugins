using Pidac.MvvmCross.Plugins.Mapping.Styling;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class FeatureLayerDescriptor : LayerDescriptor
    {
        public FeatureStylingContext StylingContext { get; set; }

        public StyleInfo StyleInfo
        {
            get { return StylingContext != null ? StylingContext.StyleInfo : null; }
        }

        public StyleType StyleType
        {
            get { return StyleInfo != null ? StyleInfo.StyleType : StyleType.NotDefined; }
        }

        public FeatureLayerDescriptor()
        {
        }

        public FeatureLayerDescriptor(StyleInfo styleInfo)
        {
            StylingContext = new FeatureStylingContext();
            StylingContext.SymbolizerInfo = new LayerSymbolizerInfo();
            StylingContext.SymbolizerInfo.Style = styleInfo;
        }
    }
}