﻿using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VLaboralApi.Models;
using VLaboralApi.Services;
using System;

namespace VLaboralApi.Providers
{
    public class BlobStorageUploadProvider : MultipartFileStreamProvider
    {
        public BlobUploadModel Upload { get; set; }

        public BlobStorageUploadProvider()
            : base(Path.GetTempPath())
        {
            Upload = new BlobUploadModel();
        }

        public override Task ExecutePostProcessingAsync()
        {
            // NOTE: FileData is a property of MultipartFileStreamProvider and is a list of multipart
            // files that have been uploaded and saved to disk in the Path.GetTempPath() location.

            foreach (var fileData in FileData)
            {
                // Sometimes the filename has a leading and trailing double-quote character
                // when uploaded, so we trim it; otherwise, we get an illegal character exception
                
                var fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));   
                fileName = Guid.NewGuid().ToString();
                // Retrieve reference to a blob
                var blobContainer = BlobHelper.GetBlobContainer();
                var blob = blobContainer.GetBlockBlobReference(fileName);

                // Set the blob content type
                blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;

                // Upload file into blob storage, basically copying it from local disk into Azure
                using (var fs = File.OpenRead(fileData.LocalFileName))
                {
                    blob.UploadFromStream(fs);
                }

                // Delete local file from disk
                File.Delete(fileData.LocalFileName);

                // Create blob upload model with properties from blob info
                var blobUpload = new BlobUploadModel
                {
                    FileName = blob.Name,
                    FileUrl = blob.Uri.AbsoluteUri,
                    FileSizeInBytes = blob.Properties.Length
                };

                // Add uploaded blob to the list
                Upload = blobUpload;
            }

            return base.ExecutePostProcessingAsync();
        }
    }
}