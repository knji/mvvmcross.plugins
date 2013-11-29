namespace Pidac.MvvmCross.Plugins.Mapping.Styling
{
    public class LineStyleInfo : StyleInfo, ILineStyleInfo
    {
        /// <summary>
        /// Color in form #FFFFFF
        /// </summary>
        public Colour StrokeColor { get; set; }
        public float StrokeWidth { get; set; }
        public LineType LineType { get; set; }
        public bool ShowPoints { get; set; }

        public LineStyleInfo()
        {
            StyleType = StyleType.Line;
            StrokeWidth = 1;
            LineType = LineType.Stroke;
        }
    }
}