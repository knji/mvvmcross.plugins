using System.Collections.Generic;

namespace MapQuest.Android.Maps
{
    public class Feature
    {
        public Feature()
        {
            ColumnValues = new Dictionary<string, string>();
        }

        public Feature(IGeometry geometry, IDictionary<string, string> columnValues = null)
        {
            Geometry = geometry;
            ColumnValues = columnValues;
        }

        public IDictionary<string, string> ColumnValues { get; private set; }
        public IGeometry Geometry { get; set; }

        public BoundingBox GetBoundingBox()
        {
            if (Geometry != null)
                return Geometry.GetBoundingBox();
            return BoundingBox.Empty;
        }
    }
}