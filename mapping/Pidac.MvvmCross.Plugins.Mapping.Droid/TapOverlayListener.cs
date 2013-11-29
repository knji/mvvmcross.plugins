using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class TapOverlayListener : Java.Lang.Object, Overlay.IOverlayTapListener
    {
        private readonly DefaultItemizedOverlay _overlay;
        private readonly AnnotationView _annotationView;

        public TapOverlayListener(DefaultItemizedOverlay overlay, AnnotationView annotationView)
        {
            _overlay = overlay;
            _annotationView = annotationView;
        }

        public void OnTap(GeoPoint p0, MapView p1)
        {
            int lastTouchedIndex = _overlay.LastFocusedIndex;
            if (lastTouchedIndex > -1)
            {
                var tapped = (OverlayItem)_overlay.GetItem(lastTouchedIndex);
                _annotationView.ShowAnnotationView(tapped);
            }
        }
    }
}