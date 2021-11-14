using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmailServerService.Model;
using EmailServerService.Requests;
using EmailServerService.Services;
using MimeKit;

namespace EmailServerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;

        private readonly IMessagingService<Message, MimeMessage> _emailService;

        public EmailController(
            ILogger<EmailController> logger,
            IMessagingService<Message, MimeMessage> emailService)
        {
            _logger = logger;

            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Post([FromForm] EmailMessageRequest request)
        {
            _logger.Log(LogLevel.Information, "Entering EmailController Post");

            var message = 
                new EmailMessage(request.To, request.Subject, request.Content, request.Attachments);

            var response = _emailService.SendMessage(request.To, message);

            _logger.Log(LogLevel.Information, "Exiting EmailController Post");

            if (!response.Valid)
            {
                return BadRequest(response);
            }

            return CreatedAtAction("Post", nameof(EmailController), message);
        }
    }
}
