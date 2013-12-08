namespace Pidac.MvvmCross.Plugins.Mapping.Geometries
{
    /// <summary>
    /// Geometry classes are modeled using OpenGIS specification:  http://dev.mysql.com/doc/refman/5.1/en/opengis-geometry-model.html 
    /// </summary>

    public abstract class Geometry : IGeometry
    {
        public abstract GeometryType GeometryType { get; }
        public abstract int Dimension { get; }

        // TODO: investigate how to use coordinate systems
        public string Srid { get; set; }
        
        public abstract bool Equals(Geometry other);
        public abstract Geometry BufferBy(double d);

        public virtual Geometry Validate()
        {
            return this;
        }

        /// <summary>
        /// Returns OGC wekk-known-text representation of this <see cref="Geometry"/>.
        /// </summary>
        /// <returns></returns>
        public abstract string AsText();
        public abstract BoundingBox GetBoundingBox();

        public virtual BoundingBox Union(Geometry theGeom)
        {
            if (theGeom == null) return GetBoundingBox();
            return GetBoundingBox().Union(theGeom.GetBoundingBox());
        }

        public static GeometryContext RemoveHeaderFromWellKnownString(string wellKnownText, string geometryType)
        {
            string innerGeom = wellKnownText;

            bool geomTypeKnown = !string.IsNullOrEmpty(geometryType);
            bool parseForInnerGeom = geomTypeKnown && StringHelper.FindPartOfString(wellKnownText, geometryType) || !geomTypeKnown;

            if (parseForInnerGeom)
            {
                var openParenIdx = wellKnownText.IndexOf('(');
                var closeParenIdx = wellKnownText.LastIndexOf(')');
                var length = closeParenIdx - openParenIdx - 1;

                if (closeParenIdx > openParenIdx)
                {
                    innerGeom = wellKnownText.Substring(openParenIdx + 1, length);
                    geometryType = StringHelper.RemoveAllSpacesFromStrng(wellKnownText.Substring(0, openParenIdx));
                }
            }

            return new GeometryContext() { Wkt = innerGeom, Type = geometryType, };
        }
    }

    public enum GeometryType
    {
        Point, LineString, Polygon,

        LinearRing,
        BoundingBox
    }
}