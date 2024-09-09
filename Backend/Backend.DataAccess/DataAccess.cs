using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Backend.Common.Interfaces;
using Backend.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess
{
    /// <inheritdoc/>
    public class DataAccess : IDisposable, IDataAccess
    {
        private DatabaseContext context;
        private bool disposed = false;
        private string storageConnectionString;
        private BlobServiceClient blobServiceClient;
       
        
        /// <inheritdoc/>
        public IRepository<ReceivedEmail> ReceivedEmails { get; }


        /// <summary>
        /// Gets the configuration
        /// </summary>
        public DataAccess(IConfiguration configuration)
        {
            this.context = new DatabaseContext(configuration);
            this.ReceivedEmails = new Repository<ReceivedEmail>(context);
            this.context.Database.EnsureCreated();
            this.storageConnectionString = configuration["StorageConnectionString"];
            this.blobServiceClient = new BlobServiceClient(this.storageConnectionString);      
        }



        /// <inheritdoc/>
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }



        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }


        /// <inheritdoc/>
        public string GetSasToken(string container, int expiresOnMinutes)
        {
            // Generates the token for this account
            var accountKey = string.Empty;
            var accountName = string.Empty;
            var connectionStringValues = this.storageConnectionString.Split(';')
                .Select(s => s.Split(new char[] { '=' }, 2))
                .ToDictionary(s => s[0], s => s[1]);
            if (connectionStringValues.TryGetValue("AccountName", out var accountNameValue) && !string.IsNullOrWhiteSpace(accountNameValue)
                && connectionStringValues.TryGetValue("AccountKey", out var accountKeyValue) && !string.IsNullOrWhiteSpace(accountKeyValue))
            {
                accountKey = accountKeyValue;
                accountName = accountNameValue;

                var storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
                var blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = container,
                    ExpiresOn = DateTime.UtcNow + TimeSpan.FromMinutes(expiresOnMinutes)
                };

                blobSasBuilder.SetPermissions(BlobAccountSasPermissions.All);
                var queryParams = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential);
                var sasToken = queryParams.ToString();
                return sasToken;
            }
            return string.Empty;
        }
    }
}
