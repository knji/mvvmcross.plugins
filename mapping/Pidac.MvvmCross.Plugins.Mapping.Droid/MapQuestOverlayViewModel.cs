using System.Collections.Generic;
using System.Collections.Specialized;
using MapQuest.Android.Maps;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public abstract class MapQuestOverlayViewModel<TOverlay, TDataManager> : MapLayerViewModel<TOverlay, TDataManager> 
        where TOverlay : Overlay
        where TDataManager : class, IGeoDataManager
    {
        public override string Name
        {
            get { return Layer.Key; }
            set { Layer.Key = value; }
        }

        protected MapQuestOverlayViewModel(string name, string alias, TOverlay layer, TDataManager dataManager)
            : base(layer, name, alias, dataManager)
        {
        }

        protected IEnumerable<GeoFeature> ConvertFeatures(IEnumerable<Feature> features)
        {
            return MapQuestGeoFeatureConverter.ConvertFeatures(features);
        }

        protected override void OnGeoDataManagerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh(DataManager.GetFeatures());
        }

        //public override BoundingBox GetBoundingBox()
        //{
        //    var bbox = DataManager.GetBounds();
        //    // use static instead of creating new object for performance?
        //    return new MapQuestGeometryConverter().ConvertBoundingBox(bbox);
        //}
    }
}