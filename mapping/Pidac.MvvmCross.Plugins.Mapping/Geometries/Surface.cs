namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    public abstract class Surface : Geometry
    {
        public override int Dimension
        {
            get
            {
                return 2;
            }
        }
    }
}