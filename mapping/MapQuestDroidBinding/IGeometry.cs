namespace MapQuest.Android.Maps
{
    public interface IGeometry
    {
        GeometryType GeometryType { get; }
        BoundingBox GetBoundingBox();
    }
}