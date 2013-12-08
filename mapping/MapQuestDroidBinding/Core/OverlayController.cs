using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using MapQuest.Android.Maps;
using MapView = MapQuest.Android.Maps.Core.MapView;
using Overlay = MapQuest.Android.Maps.Core.Overlay;

namespace MapQuest.Android.Maps.Core
{
    public class OverlayController
    {
        private IList<Overlay> _overlays = null;
        private MapView mapView;

        public OverlayController(MapView mapView)
        {
            this.mapView = mapView;
            //this._overlays = Collections.SynchronizedList(new OverlayArrayList());
        }

        public IList<Overlay> GetOverlays()
        {
            return this._overlays;
        }

        private void SetBackedList(IList<Overlay> list)
        {
            this._overlays = list;
        }

        public void RenderOverlays(Canvas canvas, MapView mapView)
        {
            if (this._overlays.Count > 0)
            {
                lock (this._overlays)
                {
                    foreach (Overlay overlay in _overlays)
                    {
                        try
                        {
                            overlay.Draw(canvas, mapView, true, mapView.DrawingTime);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    foreach (Overlay overlay in _overlays)
                    {
                        try
                        {
                            overlay.Draw(canvas, mapView, false, mapView.DrawingTime);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
        }

        public bool OnKeyDown(int keyCode, KeyEvent keyEvent, MapView mapView)
        {
            if (this._overlays.Count > 0)
            {
                lock (this._overlays)
                {
                    foreach (Overlay overlay in _overlays)
                    {
                        if (overlay.OnKeyDown(keyCode, keyEvent, mapView))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool onKeyUp(int keyCode, KeyEvent keyEvent, MapView mapView)
        {
            if (this._overlays.Count > 0)
            {
                lock (this._overlays)
                {
                    foreach (Overlay overlay in _overlays)
                    {
                        if (overlay.OnKeyUp(keyCode, keyEvent, mapView))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool OnTap(GeoPoint gp, MapView mapView)
        {
            if (this._overlays.Count > 0)
            {
                lock (this._overlays)
                {
                    foreach (Overlay overlay in _overlays)
                    {
                        if (overlay.OnTap(gp, mapView))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool OnTouchEvent(MotionEvent motionEvent, MapView mapView)
        {
            if (this._overlays.Count > 0)
            {
                lock (this._overlays)
                {
                    foreach (Overlay overlay in this._overlays)
                    {
                        if (overlay.OnTouchEvent(motionEvent, mapView))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool OnTrackballEvent(MotionEvent motionEvent, MapView mapView)
        {
            if (this._overlays.Count > 0)
            {
                lock (this._overlays)
                {
                    foreach (Overlay overlay in this._overlays)
                    {
                        if (overlay.OnTrackballEvent(motionEvent, mapView))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Destroy()
        {
            foreach (Overlay overlay in this._overlays)
            {
                overlay.Destroy();
            }
            this._overlays.Clear();
        }

        public class OverlayArrayList : List<Overlay>
        {
            private const long serialVersionUID = -1622579671240580437L;

            internal OverlayArrayList()
            {
            }

            public new void Clear()
            {
                foreach (Overlay o in this)
                {
                    o.Destroy();
                }
                base.Clear();
            }

            public Overlay Remove(int index)
            {
                var overlay = this[index];
                RemoveAt(index);
                overlay.Destroy();
                return overlay;
            }

            //public bool Remove(Object object)
            //{
            //  if ((object instanceof List)) {
            //    for (Overlay o : (List)object) {
            //      o.destroy();
            //    }
            //  } else if ((object instanceof Overlay)) {
            //    ((Overlay)object).destroy();
            //  }
            //  return super.remove(object);
            //}

            //protected void removeRange(int fromIndex, int toIndex)
            //{
            //  for (int i = fromIndex; i <= toIndex; i++) {
            //    ((Overlay)get(i)).destroy();
            //  }
            //  super.removeRange(fromIndex, toIndex);
            //}

            //public void add(int index, Overlay overlay)
            //{
            //  checkOverlayAdd(overlay);
            //  super.add(index, overlay);
            //  sort();
            //}

            //public bool add(Overlay overlay)
            //{
            //  checkOverlayAdd(overlay);
            //  bool add = super.add(overlay);
            //  sort();
            //  return add;
            //}

            //public bool addAll(Collection<? extends Overlay> collection)
            //{
            //  bool add = super.addAll(collection);
            //  sort();
            //  return add;
            //}

            //public bool addAll(int index, Collection<? extends Overlay> collection)
            //{
            //  checkOverlays(collection);
            //  bool add = super.addAll(index, collection);
            //  sort();
            //  return add;
            //}

            //private void sort()
            //{
            //  Collections.sort(this, new Comparator()
            //  {
            //    public int compare(Object x, Object xx)
            //    {
            //      int one = ((Overlay)x).getZIndex();
            //      int two = ((Overlay)xx).getZIndex();
            //      if (one == two) {
            //        return 0;
            //      }
            //      return one < two ? -1 : 1;
            //    }
            //  });
            //  EventDispatcher.sendEmptyMessage(41);
            //}

            //private void checkOverlays(Collection<? extends Overlay> collection)
            //{
            //  for (Overlay oo : collection) {
            //    checkOverlayAdd(oo);
            //  }
            //}

            //private void checkOverlayAdd(Overlay overlay)
            //{
            //  if ((overlay.getKey() == null) || (overlay.getKey().length() == 0)) {
            //    return;
            //  }
            //  Overlay o = getOverlayByKey(overlay.getKey());
            //  if (o != null) {
            //    remove(o);
            //  }
            //}

            public Overlay GetOverlayByKey(String key)
            {
                return this.FirstOrDefault(o => o.GetKey().Equals(key));
            }
        }

       
    }
}