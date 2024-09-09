using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Backend.Common.Models;
using Backend.Common.Interfaces;
using Backend.Models;
using Backend.Common.Logic;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Backend.Service.BusinessLogic
{
    /// </inheritdoc/>
    public class EmailsProcessingLogic : BaseLogic, IEmailsProcessingLogic
    {
        private readonly string azureOpenAIApiEndpoint;


        /// <summary>
        /// Gets by DI the dependeciees
        /// </summary>
        /// <param name="dataAccess"></param>
        public EmailsProcessingLogic(ISessionProvider sessionProvider, IDataAccess dataAccess, IConfiguration configuration, ILogger<IEmailsProcessingLogic> logger) : base(sessionProvider, dataAccess, logger)
        {
            this.azureOpenAIApiEndpoint = configuration["AzureOpenAIApiEndpoint"];
        }


        /// <inheritdoc/>
        public async Task<bool> ProcessEmailFromStorageAsync(Stream file, string filename)
        {
            try
            {
                this.logger.LogInformation($"Processing email from storage: {filename}");


                //await this.dataAccess.ReceivedEmails.InsertAsync(new ReceivedEmail
                //{
                //    Cmi = "CMI",
                //    ClassificationResult = "ClassificationResult",
                //    Content = "Content",
                //    Sender = "Sender",
                //    Subject = "Subject"
                //});
                //await this.dataAccess.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error processing email from storage: {ex.Message}");
            
            }
            return false;
        }
    }


}
