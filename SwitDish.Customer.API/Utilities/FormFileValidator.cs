using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Utilities
{
    public static class FormFileValidator
    {
        public static string IsValid(IFormFile file, string[] allowedExtensions, long allowedMaxSize)
        {
            if (file == null)
                return "Please upload a file";

            if (file.Length > allowedMaxSize)
                return $"File too large. Maximum size is {allowedMaxSize/(1024*1024)} MB";

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                return $"File type not allowed. Only {String.Join(',', allowedExtensions)} files are allowed";

            return "VALID";
        }
    }
}
