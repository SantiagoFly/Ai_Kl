using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Backend.Common.Extensions;
using Backend.Common.Models;
using Backend.Common.Interfaces;
using Backend.Models;
using System.IO;

namespace Backend.Service.Functions
{
    /// <summary>
    /// Documents backend API
    /// </summary>
    public class EmailsProcessing
    {
        private readonly ILogger<EmailsProcessing> logger;
        private readonly IEmailsProcessingLogic businessLogic;


        /// <summary>
        /// Receive all the depedencies by DI
        /// </summary>        
        public EmailsProcessing(IEmailsProcessingLogic businessLogic, ILogger<EmailsProcessing> logger)
        {
            this.logger = logger;
            this.businessLogic = businessLogic;
        }


        ///// <summary>
        ///// Creates a new document
        ///// </summary>       
        //[OpenApiOperation("Create", new[] { "Documents" }, Description = "Creates a new document")]
        //[OpenApiParameter("Authetication", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "User bearer token")]
        //[OpenApiSecurity("X-Functions-Key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The function key to access the API")]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Documents), Required = true, Description = "Document to create")]
        //[OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Response<Document>), Description = "The created / updated document")]
        //[Function(nameof(CreateDocumentAsync))]
        //public async Task<HttpResponseData> CreateDocumentAsync(
        // [HttpTrigger(AuthorizationLevel.Function, "post", Route = "documents")] HttpRequestData request)
        //{
        //    return await request.CreateResponse(this.businessLogic.CreateUpdateDocumentAsyc, request.DeserializeBody<Document>(), responseLinks =>
        //    {
        //        responseLinks.Links = new Dictionary<string, string> { };
        //    }, logger);
        //}


        /// <summary>
        /// Process a new email uploaded to the storage container.
        /// </summary>
        [Function(nameof(ProcessEmailFromStorageAsync))]
        public async Task ProcessEmailFromStorageAsync(
            [BlobTrigger("emails-to-process/{filename}", Connection = "AzureWebJobsStorage")] Stream file,
            string filename)
        {
            await this.businessLogic.ProcessEmailFromStorageAsync(file, filename);
        }


    }
}


