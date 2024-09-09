using Backend.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Common.Interfaces
{
    /// <summary>
    /// Data Access interface
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Documents collection
        /// </summary>
        IRepository<ReceivedEmail> ReceivedEmails { get; }


        /// <summary>
        /// Clean up resources
        /// </summary>
        void Dispose();

        /// <summary>
        /// Saves all the changess
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Get sas token
        /// </summary>
        string GetSasToken(string container, int expiresOnMinutes);

        ///// <summary>
        ///// Saves file on blob
        ///// </summary>
        //Task<string> SaveFileAsyncAsync(Guid createdById, string container, string fileName, Stream file, string contentType);

        ///// <summary>
        ///// Delete file on blob
        ///// </summary>
        //Task<bool> DeleteBlobContainer(string container, string blobName);

        ///// <summary>
        ///// Move and delete file on blob
        ///// </summary>
        //Task<bool> CopyAndDeleteBlobContainer(string sourceContainer, string destinationContarner, string oldBlobName, string newBlobName, string folder);

        //Task<bool> SaveJsonFileAsyncAsync(string container, string folder, string fileName, string content);


        //Task<string> GetJsonFileAsync(string container, string folder, string fileName);

   

       
    }
}