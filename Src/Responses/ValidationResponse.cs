using System.Collections.Generic;

namespace EmailServerService.Responses
{
    public class ValidationResponse
    {
        public ValidationResponse()
        {
            Errors = new List<ValidationResponseItem>();
        }

        public bool Valid { get; set; } = true;

        public List<ValidationResponseItem> Errors { get; set; }
    }
}
