using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Graphics;

namespace MapQuest.Android.Maps
{
    public class ZoomLevelSet
    {
        public ZoomLevelSet()
        {
            CustomZoomLevels = new Collection<ZoomLevel>();
            ZoomLevel01 = new ZoomLevel();
        }

        public Collection<ZoomLevel> CustomZoomLevels { get; private set; }
        public ZoomLevel ZoomLevel01 { get; private set; }

        public void Draw(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow)
        {
            var enumerable = features as IList<Feature> ?? features.ToList();

            if (CustomZoomLevels.Count > 0)
            {
                foreach (var zoomLevel in CustomZoomLevels)
                {
                    zoomLevel.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
                }
            }
            else
            {
                ZoomLevel01.Draw(enumerable, boundingBox, canvas, drawContext, shadow);
            }
        }
    }
}