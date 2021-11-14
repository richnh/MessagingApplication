using EmailServerService.Model;
using EmailServerService.Requests;
using EmailServerService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailServerService.Controllers
{
    public class SMSController : Controller
    {
        private readonly ILogger<SMSController> _logger;

        private readonly IMessagingService<SMSMessage, MimeMessage> _smsService;

        public SMSController(ILogger<SMSController> logger,
            IMessagingService<SMSMessage, MimeMessage> smsService)
        {
            _logger = logger;
            _smsService = smsService;
        }

        [HttpPost]
        IActionResult Post(SMSMessageRequest request)
        {
            _logger.Log(LogLevel.Information, "Entering SMSController Post");

            var message =
                new SMSMessage(request.Content);

            var response = _smsService.SendMessage(request.To, message);

            _logger.Log(LogLevel.Information, "Exiting EmailController Post");

            if (!response.Valid)
            {
                return BadRequest(response);
            }

            return CreatedAtAction("Post", nameof(SMSController), message);
        }
    }
}
