﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Access
{
    public class CloudBlobAccess
    {

        public void UploadBlob(byte[] contents, string blobName)
        {
            CloudBlobContainer container = createCloudBlobContainer(Constants.BlobNames.GetPortfolio);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            using (MemoryStream memoryStream = new MemoryStream(contents))
            {
                blockBlob.UploadFromStream(memoryStream);
            }
        }

        public void ClearBlobs()
        {
            CloudBlobContainer container = createCloudBlobContainer(Constants.BlobNames.GetPortfolio);
            container.DeleteIfExists();
            container.Create();
        }

        private CloudBlobContainer createCloudBlobContainer(string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(queueName);
            container.CreateIfNotExists();

            return container;
        }

        public byte[] DownloadPortfolio(string blobName)
        {
            CloudBlobContainer container = createCloudBlobContainer(Constants.BlobNames.GetPortfolio);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
