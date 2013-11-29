namespace Pidac.MvvmCross.Plugins.Mapping.Styling
{
    public class PointStyleInfo : StyleInfo
    {
        public PointStyleInfo(string imageUrl)
        {
            StyleType = StyleType.Point;
            ImageUrl = imageUrl;
        }

        public string ImageUrl { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
       
    }
}