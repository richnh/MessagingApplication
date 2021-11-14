
using EmailServerService.Responses;
using System.Collections.Generic;

namespace EmailServerService.Validators
{
    public interface IValidator<T>
    {
        public ValidationResponseItem Valid(string recipient, T model);

        public IList<string> ValidateRecipients(IList<string> recipients);
    }
}
