using EmailServerService.Clients;
using EmailServerService.Model;
using EmailServerService.Responses;
using EmailServerService.Validators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace EmailServerService.Services
{
    internal class SMSService : IMessagingService<SMSMessage, MailMessage>
    {
        private readonly EmailConfiguration _emailConfig;

        private readonly ILogger<SMSService> _logger;

        private readonly IValidator<SMSMessage> _validator;

        public SMSService(
            EmailConfiguration emailConfig,
            IValidator<SMSMessage> validator,
            ILogger<SMSService> logger)
        {
            _emailConfig = emailConfig;
            _validator   = validator;
            _logger      = logger;
        }

        public ValidationResponse SendMessage(List<string> recipients, SMSMessage message)
        {
            var responseInfo = new ValidationResponse();

            var validatedRecipients = _validator.ValidateRecipients(recipients);

            foreach (var mobileNumber in validatedRecipients)
            {
                var response = _validator.Valid(mobileNumber, message);

                if (!response.ErrorMessages.Any())
                {
                    var smsMessage = CreateMessage(mobileNumber, message);

                    //TODO - utlise 3rd party client
                    //var smsClient = new ThirdPartySMSClient(_emailConfig.SmtpServer);
                    //smsClient.SendMessage(mobileNumber, smsMessage);

                    Send(smsMessage);
                }
                else
                {
                    responseInfo.Valid = false;
                    responseInfo.Errors.Add(response);
                }
            }

            return responseInfo;
        }

        public MailMessage CreateMessage(string recipient, SMSMessage message)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailConfig.From);

            mailMessage.To.Add(new MailAddress(recipient));
            mailMessage.Body = message.Content;

            return mailMessage;
        }

        public void Send(MailMessage mailMessage)
        {
            var client = new SmtpClient();
            client.Send(mailMessage);
        }
    }
}
