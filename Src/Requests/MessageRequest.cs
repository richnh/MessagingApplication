using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EmailServerService.Requests
{
    public abstract class MessageRequest
    {
        [Required]
        [JsonProperty("to")]
        public List<string> To { get; set; }

        [Required]
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
