namespace Pidac.MvvmCross.Plugins.Mapping
{
    public interface IMapView
    {
        /// <summary>
        /// Return the vendor specific mapping control.
        /// </summary>
        /// <returns></returns>
        object GetMapObject();
    }
}