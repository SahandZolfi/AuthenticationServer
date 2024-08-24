using AuthenticationServer.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.IServices
{
    public interface IFileService
    {
        Task<string> UploadImage(IFormFile file, UploadImageType uploadImageType);
        Task DeleteImage(string fileName, UploadImageType uploadImageType);
        string GetImagePath(string fileName, UploadImageType uploadImageType);
    }
}
