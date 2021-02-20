using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureImgApi.Providers.FileManager
{
    public interface IStorageManager
    {
        Task<string> WriteTo(string fileName, string content);
        Task<string> WriteTo(string fileName, Stream content);
        Task<byte[]> ReadFrom(string fileName);
    }

    public class StorageManager : IStorageManager
    {
        private readonly IConfiguration _config;

        public StorageManager(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<byte[]> ReadFrom(string fileName)
        {
            try
            {
                string blobStorageConnString = _config["AzureBlobStorage:ConnectionString"].ToString();
                string containerName = _config["AzureBlobStorage:ContainerName"].ToString();

                CloudStorageAccount account = null;

                // Lookup for a CloudStorageAccount for the given ConnectionString
                // if available fetch out the Account reference
                // under the _csAccount
                if (CloudStorageAccount.TryParse(blobStorageConnString, out account))
                {
                    // create a Client to access the Containers from the Account
                    CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();

                    // get the container reference from the account via the client
                    CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

                    // check if container exists or not
                    if (!await container.ExistsAsync())
                    {
                        // create container for the given name
                        await container.CreateAsync();

                        // set the container permissions for public 
                        // restricted to blob level only
                        BlobContainerPermissions permissions = new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        };

                        await container.SetPermissionsAsync(permissions);
                    }

                    // get the blob reference - creates a blob in the container
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        // write content to the blob
                        await blockBlob.DownloadToStreamAsync(ms);
                        ms.Position = 0;
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public async Task<string> WriteTo(string fileName, Stream content)
        {
            try
            {
                string blobStorageConnString = _config["AzureBlobStorage:ConnectionString"].ToString();
                string containerName = _config["AzureBlobStorage:ContainerName"].ToString();

                CloudStorageAccount account = null;

                // Lookup for a CloudStorageAccount for the given ConnectionString
                // if available fetch out the Account reference
                // under the _csAccount
                if (CloudStorageAccount.TryParse(blobStorageConnString, out account))
                {
                    // create a Client to access the Containers from the Account
                    CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();

                    // get the container reference from the account via the client
                    CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

                    // check if container exists or not
                    if (!await container.ExistsAsync())
                    {
                        // create container for the given name
                        await container.CreateAsync();

                        // set the container permissions for public 
                        // restricted to blob level only
                        BlobContainerPermissions permissions = new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        };

                        await container.SetPermissionsAsync(permissions);
                    }

                    // get the blob reference - creates a blob in the container
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    // write content to the blob
                    await blockBlob.UploadFromStreamAsync(content);

                    return blockBlob.Uri.AbsoluteUri;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.Empty;
        }

        public async Task<string> WriteTo(string fileName, string content)
        {
            try
            {
                string blobStorageConnString = _config["AzureBlobStorage:ConnectionString"].ToString();
                string containerName = _config["AzureBlobStorage:ContainerName"].ToString();

                CloudStorageAccount account = null;

                // Lookup for a CloudStorageAccount for the given ConnectionString
                // if available fetch out the Account reference
                // under the _csAccount
                if (CloudStorageAccount.TryParse(blobStorageConnString, out account))
                {
                    // create a Client to access the Containers from the Account
                    CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();

                    // get the container reference from the account via the client
                    CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

                    // check if container exists or not
                    if (!await container.ExistsAsync())
                    {
                        // create container for the given name
                        await container.CreateAsync();

                        // set the container permissions for public 
                        // restricted to blob level only
                        BlobContainerPermissions permissions = new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        };

                        await container.SetPermissionsAsync(permissions);
                    }

                    // get the blob reference - creates a blob in the container
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    // write content to the blob
                    await blockBlob.UploadTextAsync(content);

                    return blockBlob.Uri.AbsoluteUri;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.Empty;
        }

    }
}
