using System;

namespace Backend.Models
{
    /// <summary>
    /// Email received and processed from storage container
    /// </summary>
    public class ReceivedEmail : BaseModel
    {      
        public Guid ReceivedEmailId { get; set; }

        public string Cmi { get; set; }

        public string Subject { get; set; }

        public string Sender { get; set; }

        public string ClassificationResult { get; set; }

        public string Content { get; set; }

    }
    
}


