using System;
using System.Linq;
using System.Text;

namespace Pidac.MvvmCross.Plugins.Mapping.Styling
{
    public class LayerSymbolizerInfo
    {
        public StyleInfo Style { get; set; }
        public LabelStyleInfo LabelStyleInfo { get; set; }

        public StyleType StyleType
        {
            get { return Style.StyleType; }
        }
    }

    public enum StyleType
    {
       NotDefined, Point, Line, Polygon
    }
}
