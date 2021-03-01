using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageProcessing.Core
{
    public static class Helper
    {
        public static byte[] Save(this Image image, ImageFormat format) 
        {
            using (var outStream = new MemoryStream())
            {
                var encoder = GetEncoder(image.RawFormat);
                image.Save(outStream, format);
                return outStream.ToArray();
            }
            
        }
        
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
    }
}