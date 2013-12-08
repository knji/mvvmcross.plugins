using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MapQuest.Android.Maps;

namespace MapQuest.Android.Maps
{
    public class VectorOverlay : Overlay
    {
        public FeatureSource FeatureSource { get; private set; }
        public ZoomLevelSet ZoomLevelSet { get; private set; }
        public event EventHandler BeginDraw;

        public VectorOverlay()
        {
            FeatureSource = new FeatureSource();
            ZoomLevelSet = new ZoomLevelSet();
        }

        public override void Draw(Canvas canvas, MapView mapView, bool shadow)
        {
            var beginDrawEventArgs = new OverlayDrawEventArgs();
            OnBeginDraw(beginDrawEventArgs);
            if (beginDrawEventArgs.Cancel)
                return;

            var drawContext = new DrawContext(mapView.Context.Assets, mapView);
            ZoomLevelSet.Draw(FeatureSource, FeatureSource.GetBoundingBox(), canvas, drawContext, shadow);
        }

        protected virtual void OnBeginDraw(OverlayDrawEventArgs eventArgs)
        {
            if (BeginDraw != null)
                BeginDraw(this, eventArgs);
        }

        public void Clear()
        {
            FeatureSource.DeleteAll();
        }

        public BoundingBox GetBoundingBox()
        {
            return FeatureSource.GetBoundingBox();
        }

        public void AddCustomStyle(Style style)
        {
            ZoomLevelSet.ZoomLevel01.CustomStyles.Add(style);
        }
    }

 
}