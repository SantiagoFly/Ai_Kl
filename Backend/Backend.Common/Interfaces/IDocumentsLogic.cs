using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.Common.Models;
using Backend.Models;

namespace Backend.Common.Interfaces
{
    /// <summary>
    /// Emial processing business logic
    /// </summary>
    public interface IEmailsProcessingLogic
    {
        /// <summary>
        /// Process a new email uploaded to the storage container.
        /// </summary>
        /// <returns></returns>      
        Task<bool> ProcessEmailFromStorageAsync(Stream file, string filename);
    }


}
