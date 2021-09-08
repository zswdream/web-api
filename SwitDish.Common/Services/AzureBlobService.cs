using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SwitDish.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Common.Services
{
    public class AzureBlobService : IAzureBlobStorageService
    {
        private CloudBlobClient cloudBlobClient;
        public AzureBlobService(CloudBlobClient cloudBlobClient)
        {
            this.cloudBlobClient = cloudBlobClient;
        }

        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, string fileName, IFormFile file)
        {
            try
            {
                CloudBlobContainer container = this.cloudBlobClient.GetContainerReference(blobContainerName);
         
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                await using (var data = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(data).ConfigureAwait(false);
                }

                return blockBlob.Uri;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFileBlobAsync(string blobContainerName, string fileName)
        {
            try
            {
                CloudBlobContainer cloudBlobContainer = this.cloudBlobClient.GetContainerReference(blobContainerName);

                var blob = cloudBlobContainer.GetBlobReference(fileName);
                return await blob.DeleteIfExistsAsync().ConfigureAwait(false);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
