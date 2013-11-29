namespace Pidac.MvvmCross.Plugins.Mapping.Styling
{
    public class PolygonStyleInfo : LineStyleInfo
    {
        /// <summary>
        /// Color in form #FFFFFF
        /// </summary>
        public Colour FillColor { get; set; }

        public PolygonStyleInfo()
        {
            StyleType = StyleType.Polygon;
        }
    }
}