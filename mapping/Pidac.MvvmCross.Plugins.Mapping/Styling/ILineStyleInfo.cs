namespace Pidac.MvvmCross.Plugins.Mapping.Styling
{
    public interface ILineStyleInfo
    {
        Colour StrokeColor { get; set; }
        float StrokeWidth { get; set; }
        LineType LineType { get; set; }
    }
}