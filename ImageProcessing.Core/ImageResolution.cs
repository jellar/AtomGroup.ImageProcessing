using System;
using System.Drawing;

namespace ImageProcessing.Core
{
    public static class ImageResolution
    {
        public static Image SetResizeResolution(this Image image, int width, int xDpi, int yDpi)
        {
            var options = new ImageResolutionOptions(image, width, xDpi, yDpi);
            
            return ResizeWithResolution(image, options.Width, options.Height, options.Horizontal , options.Vertical);
        }
        
        private static Image ResizeWithResolution(this Image image, int width, int height, int horizontal, int vertical)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Point[] points =
            {
                new Point(0, 0),
                new Point(width, 0),
                new Point(0, height),
            };
            graphics.DrawImage(image, points);

            bitmap.SetResolution((float)horizontal, (float)vertical);

            return bitmap;
        }
    }
    
    public class ImageResolutionOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Horizontal { get; set; }
        public int Vertical { get; set; }

        public ImageResolutionOptions(Image image, int width, int horizontal, int vertical)
        {
            Width = width > 100 ? width : 100;
            Height = (width * image.Height) / image.Width;
            Horizontal = horizontal <= 0 ? throw new Exception("The horizontal resolution must be greater than zero") : horizontal;
            Vertical = vertical <= 0 ? throw new Exception("The vertical resolution must be greater than zero") : vertical;
        }
    }
    
    
}