using System;
using System.IO;
using System.Threading.Tasks;
using AzureImgApi.Providers.FileManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureImgApi.Controllers
{
    public class AssetsModel
    {
        public IFormFile Img { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IStorageManager _storage;

        public AssetsController(IStorageManager storage)
        {
            _storage = storage;
        }

        [HttpPost, Route("upload")]
        public async Task<string> Upload([FromForm] AssetsModel model)
        {
            var file = model.Img;
            var fileName = $"{DateTime.Now.Ticks}.{GetExtension(file.FileName)}";

            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Position = 0;
                return await _storage.WriteTo(fileName, ms);
            }
        }

        private string GetExtension(string fileName)
        {
            var ext = fileName.Split(".");
            return ext[ext.Length - 1];
        }

        [HttpGet, Route("{fileName}")]
        public async Task<string> Get(string fileName)
        {
            var result = await _storage.ReadFrom(fileName);
            // var data = $"data:image/jpeg;base64, {Convert.ToBase64String(result)}";
            var data = Convert.ToBase64String(result);
            return data;
        }
    }
}