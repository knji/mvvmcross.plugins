using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping.CoordinateSystems
{
    public abstract class CoordinateSystem : ICoordinateSystem
    {
        public BoundingBox DefaultBounds { get; protected set; }
        public string Name { get; private set; }
        public string Authority { get; private set; }
        public int AuthorityCode { get; private set; }

        protected CoordinateSystem(string name, string authority, int authorityCode)
        {
            Name = name;
            Authority = authority;
            AuthorityCode = authorityCode;
        }
    }
}