using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Apparent.Model;

namespace Apparent
{
    public class StorageService
    {
        private readonly string BlobStorageConnection;

        public StorageService() {
            BlobStorageConnection = ConfigurationManager.AppSettings.Get("BlobStorageConnection");

        }

        public async Task<bool> DeleteFile(string containerName, string blobName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(BlobStorageConnection, containerName);
            bool blobContainerExist = await blobContainerClient.ExistsAsync();
            if (blobContainerExist)
            {
                BlobClient client = blobContainerClient.GetBlobClient(blobName);
                return await client.DeleteIfExistsAsync();
            }
            return false;
        }


        public async Task<BlobResult> UploadFile(Stream fileStream, string containerName, string blobName)
        {
            BlobResult blobResult = new BlobResult();
            try
            {
                BlobContainerClient containerClient = new BlobContainerClient(BlobStorageConnection, containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                await blobClient.UploadAsync(fileStream);


                blobResult.Uri = blobClient.Uri.AbsoluteUri;
                blobResult.BlobAccountName = blobClient.AccountName;
                    blobResult.BlobName = blobClient.Name;
                blobResult.BlobContainerName = blobClient.BlobContainerName;
                
                return blobResult;
            }
            catch(Exception ex) {
                return blobResult;
            };
           
        }

    }
}