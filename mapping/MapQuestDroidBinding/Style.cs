using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MapQuest.Android.Maps
{
    public abstract class Style
    {
        public Rect Bounds { get; private set; }
        public ICollection<string> RequiredColumns { get; private set; }

        protected Style()
        {
            Bounds = new Rect(); 
            RequiredColumns = new Collection<string>();
        }

        public void Draw(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow)
        {
            DrawCore(features, boundingBox, canvas, drawContext, shadow);
        }

        public abstract void DrawCore(IEnumerable<Feature> features, BoundingBox boundingBox, Canvas canvas, DrawContext drawContext, bool shadow);
        
        protected abstract bool CanDraw(Feature feature);

        protected void DrawAt(Canvas canvas, Drawable drawable, int x, int y, bool shadow)
        {
            try
            {
                canvas.Save();
                canvas.Translate(x, y);
                if (shadow)
                {
                    drawable.SetColorFilter(Util.Int32ToColor(2130706432), PorterDuff.Mode.SrcIn);
                    canvas.Skew(-0.9F, 0.0F);
                    canvas.Scale(1.0F, 0.5F);
                }

                drawable.Draw(canvas);
                if (shadow)
                {
                    drawable.ClearColorFilter();
                }
            }
            finally
            {
                canvas.Restore();
            }
        }
      
    }
}