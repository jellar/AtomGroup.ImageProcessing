using System;
using System.Drawing;

namespace ImageProcessing.Core
{
    public static class Watermark
    {
        public static Image SetWatermarkText(this Image image, string watermarkText)
        {
            return WithWatermarkText(image, watermarkText);
        }

        private static Image WithWatermarkText(Image image, string watermarkText)
        {
            Bitmap bitmap = new Bitmap(image);
            Graphics graphics = Graphics.FromImage(bitmap);
            
            Font font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
            Color color = Color.FromArgb(100, 0, 0, 0);
            
            for (var i = 50; i > 0; i--)
            {
                font = new Font("Arial", i, FontStyle.Bold);
                SizeF sizef = graphics.MeasureString(watermarkText, font, int.MaxValue);

                var sin = Math.Sin(Math.PI / 180);
                var cos = Math.Cos(Math.PI / 180);
                var opp1 = sin * sizef.Width;
                var adj1 = cos * sizef.Height;
                var opp2 = sin * sizef.Height;
                var adj2 = cos * sizef.Width;

                if (opp1 + adj1 < image.Height && opp2 + adj2 < image.Width)
                    break;
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            graphics.DrawString(watermarkText, font, new SolidBrush(color), new Point(image.Width/2, image.Height/2), sf);
            return bitmap;
        }
    }
}