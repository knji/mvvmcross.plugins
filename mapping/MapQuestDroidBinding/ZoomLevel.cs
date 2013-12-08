using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Graphics;

namespace MapQuest.Android.Maps
{
    public class ZoomLevel
    {
        public ZoomLevel() : this(0)
        {
        }

        public ZoomLevel(double scale)
        {
            Scale = scale;
            CustomStyles = new Collection<Style>();
            DefaultPointStyle = new PointStyle();
        }

        public double Scale { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public PointStyle DefaultPointStyle { get; set; }
        public LineStyle DefaultLineStyle { get; set; }
        public AreaStyle DefaultAreaStyle { get; set; }
        public TextStyle DefaultTextStyle { get; set; }
        public ZoomLevel ApplyUntil { get; set; }
        public Collection<Style> CustomStyles { get; private set; }

        public void Draw(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow)
        {
            var enumerable = features as IList<Feature> ?? features.ToList();
            if (CustomStyles.Count > 0)
            {
                foreach (var style in CustomStyles)
                {
                    style.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
                }
            }
            else
            {
                DefaultPointStyle.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
                DefaultLineStyle.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
                DefaultAreaStyle.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
                DefaultTextStyle.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
            }
        }
    }
}