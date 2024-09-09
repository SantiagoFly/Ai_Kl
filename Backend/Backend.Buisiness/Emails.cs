using System;
using System.IO;
using MimeKit;

class Program
{
    static void Main(string[] args)
    {
        string emlFilePath = @"C:\Users\flynn\Desktop\Correos Klog\Test\correo1.eml";
        string pdfFilePath = @"C:\Users\flynn\Desktop\Correos Klog\Test";

        // Cargar el archivo .eml utilizando MimeKit
        var message = MimeMessage.Load(emlFilePath);

      
        string subject = message.Subject;

        
        string body = GetBody(message);

        Console.Write("Asunto del Correo: ");
        Console.WriteLine(subject);
        Console.WriteLine("Cuerpo del Correo:");
        Console.WriteLine(body);

        //  si el correo tiene archivos adjuntos
        foreach (var attachment in message.Attachments)
        {
            //  si el adjunto es un archivo PDF
            if (attachment is MimePart pdfAttachment && IsPdfAttachment(pdfAttachment))
            {
                // Guardar el archivo PDF
                string pdfFileName = pdfAttachment.FileName;
                string pdfFullPath = Path.Combine(pdfFilePath, pdfFileName);
                using (var fileStream = File.Create(pdfFullPath))
                {
                    pdfAttachment.Content.DecodeTo(fileStream);
                }

                Console.WriteLine($"Se ha extraído el archivo PDF: {pdfFullPath}");
            }
        }
    }

    static string GetBody(MimeMessage message)
    {
        //se puede acceder a message.TextBody o message.HtmlBody 
        return message.HtmlBody;
    }

    static bool IsPdfAttachment(MimePart attachment)
    {
        return attachment.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
    }
}
