using EmailServerService.Model;
using EmailServerService.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace EmailServerService.Validators
{
    public class EmailValidator : IValidator<EmailMessage>
    {
        private EmailAddressAttribute _emailAddressAttribute;

        private ValidationResponseItem _validationResponseItem;

        public EmailValidator()
        {
            _emailAddressAttribute = new EmailAddressAttribute();
            _validationResponseItem = new ValidationResponseItem();
        }

        public ValidationResponseItem Valid(string recipient, EmailMessage model)
        {
            if(!_emailAddressAttribute.IsValid(recipient))
            {
                _validationResponseItem.ErrorMessages
                    .Add(string.Format("Invalid email address : {0}", recipient));
            }

            if(String.IsNullOrEmpty(model.Content))
            {
                _validationResponseItem.ErrorMessages
                    .Add("Email content cannot be null or empty : {0}");
            }

            return _validationResponseItem;
        }

        public IList<string> ValidateRecipients(IList<string> recipients)
        {
            return recipients.Distinct().Where(x => !String.IsNullOrEmpty(x)).ToList();
        }
    }
}
