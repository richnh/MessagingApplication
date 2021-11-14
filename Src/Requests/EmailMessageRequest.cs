using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EmailServerService.Requests
{
    public class EmailMessageRequest : MessageRequest
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("attachments")]
        public List<IFormFile> Attachments { get; set; }
    }
}
