using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageProcessing.Core
{
    public static class Helper
    {
        public static byte[] SaveAs(this Image image, ImageFormat format) 
        {
            using (var outStream = new MemoryStream())
            {
                var encoder = GetEncoder(image.RawFormat);
                image.Save(outStream, format);
                return outStream.ToArray();
            }
        }
        
        public static byte[] ImageToByteArray(Image image, string path)
        {
            using (var ms = new MemoryStream())
            {

                var extension = "." + path.Split('.')[path.Split('.').Length - 1];
                var myImageCodecInfo = GetEncoderInfo(extension);
                image.Save(ms,myImageCodecInfo, null); 
                return  ms.ToArray();
            }
        }
        
        public static ImageCodecInfo GetEncoder(ImageFormat format)
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
        
        public static ImageCodecInfo GetEncoderInfo(string ext)
        {
            int j;

            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].FilenameExtension.ToLower().Contains(ext.ToLower()))
                    return encoders[j];
            }

            throw new Exception("Encoder not found");
        }

        public static Color GetColorFromName(string name)
        {
            var color = Color.FromName(name);
            return !color.IsKnownColor ? Color.LightGray : color;
        }

        public static bool VerifyImageFileExtension(string fileExtension)
        {
            string[] fileTypes = {".jpg", ".jpeg", ".png", ".gif"};
            return fileTypes.Any(t => fileExtension == t);
        }

        public static string GetFileContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default: 
                    return "image/png";
            }
        }
    }
}