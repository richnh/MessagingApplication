using EmailServerService.Clients;
using EmailServerService.Model;
using EmailServerService.Responses;
using EmailServerService.Validators;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;

[assembly: InternalsVisibleTo("MessagingService.Test")]
namespace EmailServerService.Services
{
    internal class EmailService : IMessagingService<EmailMessage, MimeMessage>
    {
        private readonly ILogger<EmailService> _logger;

        private readonly EmailConfiguration _emailConfig;

        private readonly IValidator<EmailMessage> _validator;

        public EmailService(
            EmailConfiguration emailConfig,
            IValidator<EmailMessage> validator,
            ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig;
            _validator   = validator;
            _logger      = logger;
        }

        public ValidationResponse SendMessage(List<string> recipients, EmailMessage message)
        {
            _logger.Log(LogLevel.Information, "Entering EmailService SendMessage()");

            var responseInfo = new ValidationResponse();

            var validatedRecipients = _validator.ValidateRecipients(recipients);

            foreach (var emailAddress in validatedRecipients)
            {
                var response = _validator.Valid(emailAddress, message);

                if(!response.ErrorMessages.Any())
                {
                    var emailMessage = CreateMessage(emailAddress, message);

                    //TODO - utlise 3rd party client
                    //var emailClient = new ThirdPartyEmailClient(_emailConfig.SmtpServer);
                    //emailClient.SendMessage(recipients.ToArray(), emailMessage);
                    Send(emailMessage);
                }
                else
                {
                    responseInfo.Valid = false;
                    responseInfo.Errors.Add(response);
                }
            }

            _logger.Log(LogLevel.Information, "Exiting EmailService SendMessage()");

            return responseInfo;
        }

        public MimeMessage CreateMessage(string to, EmailMessage message)
        {
            _logger.Log(LogLevel.Information, "Entering EmailService CreateMessage()");

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = message.Subject;      

            var bodyBuilder = new BodyBuilder { HtmlBody =  message.Content };

            if(message.Attachments != null)
            {
                byte[] fileBytes;

                foreach(var attachment in message.Attachments)
                {
                    using(var ms = new MemoryStream() )
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            _logger.Log(LogLevel.Information, "Exiting EmailService CreateMessage()");

            return mimeMessage;
        }

        public void Send(MimeMessage mailMessage)
        {
            _logger.Log(LogLevel.Information, "Entering EmailService Send()");

            using (var client = new SmtpClient())
            {
                try
                {
                    client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    client.CheckCertificateRevocation = false;
                    client.Connect(_emailConfig.SmtpServer, 25, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                } 
            }

            _logger.Log(LogLevel.Information, "Exiting EmailService Send()");
        }
    }
}
