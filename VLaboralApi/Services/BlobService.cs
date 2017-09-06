using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VLaboralApi.Models;
using VLaboralApi.Providers;

namespace VLaboralApi.Services
{
    public interface IBlobService
    {
        Task<BlobUploadModel> UploadBlobs(HttpContent httpContent);
        Task<BlobDownloadModel> DownloadBlob(string blobId);
    }

    public class BlobService : IBlobService
    {
        public async Task<BlobUploadModel> UploadBlobs(HttpContent httpContent)
        {
            var blobUploadProvider = new BlobStorageUploadProvider();

            var file = await httpContent.ReadAsMultipartAsync(blobUploadProvider)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        throw task.Exception;
                    }

                    var provider = task.Result;
                    return provider.Upload;
                });

            // TODO: Use data in the list to store blob info in your
            // database so that you can always retrieve it later.

            return file;
        }

        private VLaboral_Context db = new VLaboral_Context();
        public async Task<BlobDownloadModel> DownloadBlob(string blobId)
        {
            // TODO: You must implement this helper method. It should retrieve blob info
            // from your database, based on the blobId. The record should contain the
            // blobName, which you should return as the result of this helper method.
            var blobPrm = db.BlobUploadModels
                .Where(b => b.FileName == blobId)
                .FirstOrDefault();
            if (blobPrm == null) {
                return null;
            }

            var blobName = blobPrm.FileName; //GetBlobName(blobId);

            if (!String.IsNullOrEmpty(blobName))
            {
                var container = BlobHelper.GetBlobContainer();
                var blob = container.GetBlockBlobReference(blobName);

                // Download the blob into a memory stream. Notice that we're not putting the memory
                // stream in a using statement. This is because we need the stream to be open for the
                // API controller in order for the file to actually be downloadable. The closing and
                // disposing of the stream is handled by the Web API framework.
                var ms = new MemoryStream();
                await blob.DownloadToStreamAsync(ms);

                // Strip off any folder structure so the file name is just the file name
                var lastPos = blob.Name.LastIndexOf('/');
                var fileName = blob.Name.Substring(lastPos + 1, blob.Name.Length - lastPos - 1);

                // Build and return the download model with the blob stream and its relevant info
                var download = new BlobDownloadModel
                {
                    BlobStream = ms,
                    BlobFileName = fileName,
                    BlobLength = blob.Properties.Length,
                    BlobContentType = blob.Properties.ContentType
                };

                return download;
            }

            // Otherwise
            return null;
        }
    }
}