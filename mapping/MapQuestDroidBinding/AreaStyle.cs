using System;
using System.Collections.Generic;
using Android.Graphics;

namespace MapQuest.Android.Maps
{
    public class AreaStyle : Style
    {
        public override void DrawCore(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow)
        {
            throw new NotImplementedException();
        }

        protected override bool CanDraw(Feature feature)
        {
            return feature.Geometry.GeometryType == GeometryType.Polygon;
        }
    }
}