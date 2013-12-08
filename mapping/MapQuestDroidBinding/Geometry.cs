namespace MapQuest.Android.Maps
{
    public abstract class Geometry : IGeometry
    {
        public abstract GeometryType GeometryType { get; }
        public abstract int Dimension { get; }

        // TODO: investigate how to use coordinate systems
        public string Srid { get; set; }

        public abstract bool Equals(Geometry other);
        public abstract IGeometry BufferBy(double d);

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
    }
}