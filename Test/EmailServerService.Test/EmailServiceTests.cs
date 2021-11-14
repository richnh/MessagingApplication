using EmailServerService.Services;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using EmailServerService.Validators;
using EmailServerService.Model;
using System.Collections.Generic;

namespace EmailServerService.Test
{
    public class EmailServiceTests
    {
        [Fact]
        public void SendMessage()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void Send()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void CreateMessage()
        {
            // Arrange
            var config = new Mock<EmailConfiguration>();
            config.Object.From = "test@outlook.com";

            var logger 
                = new Mock<ILogger<EmailService>>();

            var validator 
                = new Mock<IValidator<EmailMessage>>();

            var expectedMessage 
                = new EmailMessage(new List<string>() { "bob@tmail.com" }, "Test Subject", "Test Content");

            var service
                = new EmailService(config.Object, validator.Object, logger.Object);

            // Act
            var createdMessage = service.CreateMessage("bob@tmail.com", expectedMessage);

            // Assert
            Assert.Equal(expectedMessage.Subject, createdMessage.Subject);
            Assert.Equal(expectedMessage.Content, createdMessage.HtmlBody);
        }
    }
}
