using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ImageProcessing.Api.ViewModels
{
    public class UploadImageModel
    {
        [Required]
        public IFormFile File { get; set; }
        
        [Required]
        public int Width { get; set; }

        [Required]
        public int XDpi { get; set; }
        
        [Required]
        public int YDpi { get; set; }
        
        public string BackGroundColor { get; set; }
        public string WatermarkText { get; set; }
        [Required]
        public string Extension { get; set; }

        public override string ToString()
        {
            return $"{File.FileName}-{Width}-{XDpi}-{YDpi}-{BackGroundColor}-{WatermarkText}-{Extension}";
        }
    }
}