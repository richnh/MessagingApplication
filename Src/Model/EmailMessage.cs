using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;

namespace EmailServerService.Model
{
    public class EmailMessage : Message
    {
        [JsonProperty("subject")]
        public string Subject { get; private set; }

        [JsonProperty("attachments")]
        public List<IFormFile> Attachments { get; set; }

        public EmailMessage(List<string> to, string subject, string content, List<IFormFile> attachments = null)
        {
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }
}
