using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing.Core
{
    public static class BackgroundColor
    {
        public static Image SetBackgroundColor(this Image image, Color color)
        {
            return WithBackgroundColor(image, color);
        }
        
        private static Image WithBackgroundColor(Image image, Color color)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f; 

            ColorMatrix cm = new ColorMatrix(new float[][]
            {
                new float[] {r, 0, 0, 0, 0},
                new float[] {0, g, 0, 0, 0},
                new float[] {0, 0, b, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });
            ImageAttributes imAttribute = new ImageAttributes();
            imAttribute.SetColorMatrix(cm);
            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            using (Graphics graphics1 = Graphics.FromImage(bitmap))
            {
                graphics1.DrawImage(image, points, rect, GraphicsUnit.Pixel, imAttribute);
            }

            return bitmap;
        }
    }
}