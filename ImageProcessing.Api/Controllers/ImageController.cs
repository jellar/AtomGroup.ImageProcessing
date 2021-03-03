using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessing.Api.ViewModels;
using ImageProcessing.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace ImageProcessing.Api.Controllers
{
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public ImageController(IDistributedCache cache)
        {
            _cache = cache;
        }
        // POST
        [HttpPost("upload")]
        public IActionResult Upload([FromForm]UploadImageModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new {message = "Required parameters are missing"});
           
            if(!CheckIfImageFile(model.File)) return BadRequest(new { message = "Invalid file extension" });
            var fileName = DateTime.Now.Ticks + model.Extension;
            
            var extension = "." + model.File.FileName.Split('.')[model.File.FileName.Split('.').Length - 1];
            var contentType = Helper.GetFileContentType(extension);
            
            var data = _cache.Get(model.ToString());
            if (data == null)
            {
                var processedImage = ProcessImage(model, fileName);

                return Ok(File(processedImage, contentType, fileName ));
            }

            return Ok(File(data, contentType, fileName ));
        }
        
        private bool CheckIfImageFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return Helper.VerifyImageFileExtension(extension); // Change the extension based on your need
        }

        private byte[] ProcessImage(UploadImageModel model, string fileName)
        {
            
            using var fileStream = model.File.OpenReadStream();
            byte[] bytes = new byte[model.File.Length];
            fileStream.Read(bytes, 0, (int)model.File.Length);
            
            using (MemoryStream inStream = new MemoryStream(bytes))
            {
                var img = Image.FromStream(inStream);
                var scaledImage = img.SetResizeResolution(model.Width, model.XDpi, model.YDpi);
                if (!string.IsNullOrEmpty(model.WatermarkText))
                {
                    scaledImage = scaledImage.SetWatermarkText(model.WatermarkText);
                }
                if (!string.IsNullOrEmpty(model.BackGroundColor))
                {
                    scaledImage = scaledImage.SetBackgroundColor(model.BackGroundColor);
                }
                
                var processedImage = Helper.ImageToByteArray(scaledImage, fileName);
                
                _cache.Set(model.ToString(), processedImage);
                
                WriteFile(processedImage, fileName);

                return processedImage;
            }
        }
        
        private void WriteFile(byte[] image, string extension)
        {
            try
            {
                //var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extension;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                    fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(image, 0, image.Length);
                    //file.CopyTo(stream);
                }

            }
            catch (Exception )
            {
                //log error
            }

        }
    }

    
}