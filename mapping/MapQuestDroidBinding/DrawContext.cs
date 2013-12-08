using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;

namespace MapQuest.Android.Maps
{
    public class DrawContext
    {
        public DrawContext(AssetManager assets, MapView mapView)
        {
            Assets = assets;
            MapView = mapView;
            //Canvas = drawCanvas;
        }

        //public Canvas Canvas { get; private set; }
        public AssetManager Assets { get; private set; }
        public MapView MapView { get; private set; }
        //public IEnumerable<Feature> FeaturesToDraw { get; private set; }
        //public BoundingBox FeatureBounds { get; private set; }
    }
}