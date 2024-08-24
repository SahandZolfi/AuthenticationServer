using AuthenticationServer.Core.IServices;
using AuthenticationServer.Shared.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string? _baseAddress = "";

        public FileService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
            _baseAddress = _configuration["BaseAddress"];
        }

        public async Task<string> UploadImage(IFormFile file, UploadImageType uploadImageType)
        {
            string imageName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            string imagePath = "";
            switch (uploadImageType)
            {
                case UploadImageType.UserProfile:
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", "UserProfileImages", imageName);
                    break;
            }
            using (var stream = System.IO.File.Create(imagePath))
            {
                await file.CopyToAsync(stream);
            }
            return imageName;
        }

        public string GetImagePath(string fileName, UploadImageType uploadImageType)
        {
            throw new NotImplementedException();
        }
        
        public async Task DeleteImage(string fileName, UploadImageType uploadImageType)
        {
            string imagePath = "";
            switch (uploadImageType)
            {
                case UploadImageType.UserProfile:
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", "UserProfileImages", fileName);
                    break;
            }
            File.Delete(imagePath);
        }
    
    }
}
