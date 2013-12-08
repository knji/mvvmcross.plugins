using System;
using System.Collections.Generic;
using Android.Graphics;

namespace MapQuest.Android.Maps
{
    public class TextStyle : Style
    {
        public override void DrawCore(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow)
        {
            throw new NotImplementedException();
        }

        protected override bool CanDraw(Feature feature)
        {
            // TODO: add some other condition
            return feature.ColumnValues != null && feature.ColumnValues.Count > 0;
        }
    }
}