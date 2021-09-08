using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Common.Interfaces
{
    public interface IAzureBlobStorageService
    {
        public Task<Uri> UploadFileBlobAsync(string blobContainerName, string fileName, IFormFile file);
        public Task<bool> DeleteFileBlobAsync(string blobContainerName, string fileName);
    }
}
