using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace Backend.BusinessLogic
{
    public class EmailBusinessLogic : IEmailBusinessLogic
    {
        private readonly ILogger<EmailBusinessLogic> _logger;
        private readonly EmailExtractor _emailExtractor;
        private readonly BlobServiceClient _blobServiceClient;

        public EmailBusinessLogic(ILogger<EmailBusinessLogic> logger, EmailExtractor emailExtractor, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _emailExtractor = emailExtractor;
            _blobServiceClient = blobServiceClient;
        }

        public async Task ProcessBlobAsync(Stream blobStream, string blobName)
        {
            try
            {
                _logger.LogInformation($"Blob trigger function Processed blob\n Name:{blobName} \n Size: {blobStream.Length} Bytes");

                // Extraer contenido del correo electrónico y archivo PDF adjunto
                var emailContent = _emailExtractor.ExtractEmailContent(blobStream);
                if (emailContent != null)
                {
                    var (subject, body, pdfStream) = emailContent.Value;

                    // Loggear el contenido del correo
                    _logger.LogInformation($"Asunto del Correo: {subject}");
                    _logger.LogInformation("Cuerpo del Correo:");
                    _logger.LogInformation(body);

                    // Aca falta conseguir el remitente del correo
                    var sender = "Desconocido";
                    var date = DateTime.Now.ToString("yyyyMMdd");

                   
                    if (sender != null)
                    {
                        sender = string.Join("_", sender.Split(Path.GetInvalidFileNameChars()));
                    }

                    // Crear el nombre de archivo .eml con el formato sender_date.eml
                    var fileName = $"{sender}_{date}.eml";

                    // Obtener el contenedor de Blob donde se almacenarán los archivos
                    var containerClient = _blobServiceClient.GetBlobContainerClient("Booking");

                    // Subir el archivo .eml al Blob Storage
                    var emlBlobClient = containerClient.GetBlobClient(fileName);
                    await emlBlobClient.UploadAsync(blobStream);
                    // Iterar a través de los adjuntos del correo electrónico y subir los archivos PDF al Blob Storage
                    foreach (var attachment in message.Attachments)
                    {
                        // Verificar si el adjunto es un archivo PDF
                        if (attachment is MimePart pdfAttachment && IsPdfAttachment(pdfAttachment))
                        {
                            // Crear el nombre de archivo PDF con el formato sender_date.pdf
                            var pdfFileName = $"{sender}_{date}.pdf";

                            // Obtener el stream del archivo PDF adjunto
                            using (var pdfMemoryStream = new MemoryStream())
                            {
                                pdfAttachment.Content.DecodeTo(pdfMemoryStream);
                                pdfMemoryStream.Position = 0;

                                // Subir el archivo PDF al Blob Storage
                                var pdfBlobClient = containerClient.GetBlobClient(pdfFileName);
                                await pdfBlobClient.UploadAsync(pdfMemoryStream);
                            }

                            _logger.LogInformation($"Se ha subido el archivo PDF: {pdfFileName}");
                        }
                    }

                    _logger.LogInformation($"Se ha subido el archivo .eml: {fileName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al procesar el blob '{blobName}'");
                    throw;
                }
            }

        static string GetBody(MimeMessage message)
        {
            // Se puede acceder a message.TextBody o message.HtmlBody 
            return message.HtmlBody;
        }

        static bool IsPdfAttachment(MimePart attachment)
        {
            return attachment.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
        }
    }
}
