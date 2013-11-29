using Pidac.MvvmCross.Plugins.Mapping.Geometries;
using Pidac.MvvmCross.Plugins.Mapping.Styling;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public interface IMapViewController
    {
        LayerViewModels LayerViewModels { get; }

        void SetActiveMap(IMapView view, Point center, int zoomLevel);

        /// <summary>
        /// This should add a feature layer that represents user's current location to track user movement.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="userSymbol"></param>
        /// <param name="compassSymbol"></param>
        /// <param name="userLocationDataManager"></param>
        void AddMyLocationLayer(string name, string alias,
            PointStyleInfo userSymbol,
            PointStyleInfo compassSymbol = null,
            IUserLocationTracker userLocationDataManager = null);

        /// <summary>
        /// Create the feature layer.  If may is not yet ready, queue this request and 
        /// when map becomes ready, create the layer.
        /// </summary>
        /// <param name="layerContext"></param>
        void AddFeatureLayer(FeatureLayerContext layerContext);
        
        /// <summary>
        /// This method will be called after the map has loaded so that any pending 
        /// layers that may have been submitted for creation through the CreateFeatureLayer
        /// can be serviced.
        /// </summary>
        void DrawPendngLayers();
        void DisposeActiveMapContext();
    }
}
