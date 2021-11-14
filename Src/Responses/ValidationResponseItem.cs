using System.Collections.Generic;

namespace EmailServerService.Responses
{
    public class ValidationResponseItem
    {
        public ValidationResponseItem()
        {
            ErrorMessages = new List<string>();
        }

        public List<string> ErrorMessages { get; set; }
    }
}
