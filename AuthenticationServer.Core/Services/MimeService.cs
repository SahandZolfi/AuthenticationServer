using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.Services
{
    public class MimeService
    {
        public static bool IsImageFileTypeValid(FileInfo fileInfo)
        {
            if (fileInfo.Extension.ToLower() == ".jpg" || fileInfo.Extension.ToLower() == ".png" || fileInfo.Extension.ToLower() == ".jpeg")
                return true;
            else
                return false;
        }
    }
}
