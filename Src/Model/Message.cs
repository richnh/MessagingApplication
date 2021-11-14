using Newtonsoft.Json;
using System.Text.Json;

namespace EmailServerService.Model
{
    public abstract class Message
    {
        [JsonRequired]
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
