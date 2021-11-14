using EmailServerService.Model;
using EmailServerService.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace EmailServerService.Validators
{
    public class SMSValidator : IValidator<SMSMessage>
    {
        private ValidationResponseItem _validationResponseItem;

        public SMSValidator()
        {
            _validationResponseItem = new ValidationResponseItem();
        }

        public ValidationResponseItem Valid(string recipient, SMSMessage model)
        {
            if(String.IsNullOrEmpty(model.Content))
            {
                _validationResponseItem.ErrorMessages
                    .Add("SMS content cannot be null or empty : {0}");
            }

            return _validationResponseItem;
        }

        public IList<string> ValidateRecipients(IList<string> recipients)
        {
            return recipients.Distinct().Where(x => !String.IsNullOrEmpty(x)).ToList();
        }
    }
}
