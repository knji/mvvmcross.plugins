using Pidac.MvvmCross.Plugins.Mapping.Styling;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public class FeatureStylingContext
    {
        public StyleType StyleType
        {
            get { return SymbolizerInfo.StyleType; }
        }

        public LayerSymbolizerInfo SymbolizerInfo { get; set; }
        public StyleInfo StyleInfo { get { return SymbolizerInfo.Style; } }
    }
}