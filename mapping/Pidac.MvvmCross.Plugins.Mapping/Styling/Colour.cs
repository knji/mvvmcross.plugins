namespace Pidac.MvvmCross.Plugins.Mapping.Styling
{
    public struct Colour
    {
        public Colour(byte r, byte g, byte b, byte a) : this()
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }
}