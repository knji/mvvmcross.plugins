using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using MapQuest.Android.Maps;
using Pidac.MvvmCross.Plugins.Mapping.Styling;

namespace Pidac.MvvmCross.Plugins.Mapping.Droid
{
    public class MapquestStyleFactory
    {
        public static Style Create(StyleInfo styleInfo, AssetManager assetManager)
        {
            if (styleInfo.StyleType == StyleType.Point)
            {
                return CreatePointStyle((PointStyleInfo) styleInfo, assetManager);
            }

            throw new NotImplementedException();
        }

        public static PointStyle CreatePointStyle(PointStyleInfo styleInfo, AssetManager assetManager)
        {
            Drawable drawable = null;
            if (!string.IsNullOrWhiteSpace(styleInfo.ImageUrl))
            {
                drawable = CreateBitmapDrawable(styleInfo.ImageUrl, assetManager);
            }
            else
            {
                drawable = new ColorDrawable(CreateColor(styleInfo.SolidColor));    
            }

            return new PointStyle(drawable);
        }

        public static Drawable CreateBitmapDrawable(string imageUrl, AssetManager assetManager)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new NotSupportedException("point style for non-images not supported.");
            }

            var asset = assetManager.Open(imageUrl);
            return new BitmapDrawable(asset);
        }

        public static Drawable CreateDrawable(PointStyleInfo styleInfo, AssetManager assetManager )
        {
            if (string.IsNullOrWhiteSpace(styleInfo.ImageUrl))
            {
                throw new NotSupportedException("point style for non-images not supported.");
            }

            var asset = assetManager.Open(styleInfo.ImageUrl);
            return new BitmapDrawable(asset);
        }

        public static Color CreateColor(Colour color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static LineStyle CreateLineStyle(LineStyleInfo styleInfo)
        {
            var style = new LineStyle
                {
                    LinePaint = CreatePaintFromStyleInfo(styleInfo),
                    ShowPoints = false
                };

            return style;
        }

        public static Paint.Style GetLineStyle(LineType lineType)
        {
            if (lineType == LineType.Solid)
                return Paint.Style.Fill;
            if (lineType == LineType.Stroke)
                return Paint.Style.Stroke;
            return Paint.Style.Fill;
        }

        public static Paint CreatePaintFromStyleInfo(ILineStyleInfo styleInfo)
        {
            var paint = new Paint(PaintFlags.AntiAlias);
            paint.StrokeWidth = styleInfo.StrokeWidth;
            paint.Alpha = 100;
            paint.Color = CreateColor(styleInfo.StrokeColor);
            paint.SetStyle(GetLineStyle(styleInfo.LineType));

            return paint;
        }
    }
}