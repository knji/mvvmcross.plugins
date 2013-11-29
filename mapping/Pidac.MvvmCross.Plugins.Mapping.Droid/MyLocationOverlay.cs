using System;
using System.Collections.Generic;
using System.Text;
using Android.Graphics.Drawables;
using Android.Runtime;
using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MyLocationOverlay : DefaultItemizedOverlay
    {
        protected MyLocationOverlay(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MyLocationOverlay(Drawable yourLocationImage, Drawable compassMarker = null)
            : base(yourLocationImage)
        {
            CompassMarker = compassMarker;
        }

        //// For location updates, it is best to pass an interface that provides this information???
        public bool IsLocationUpdateEnabled { get; set; }
        public Drawable CompassMarker { get; set; }
        public bool IsCompassEnabled { get; set; }
        public bool IsFollowing { get; set; }
        //public GeoPoint LastLocation { get; private set; }
       // public IUserLocationTracker UserLocationTracker { get; set; }


    }
}
