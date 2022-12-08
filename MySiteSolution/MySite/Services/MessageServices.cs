using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyCatalogSite.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly ILogger<AuthMessageSender> logger;

        public AuthMessageSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor,
            ILogger<AuthMessageSender> logger)
        {
            this.Options = optionsAccessor.Value;
            this.logger = logger;
        }

        public AuthMessageSenderOptions Options { get; }

        public async Task SendEmailAsync(string email, string subject, string message)
        {

            if (string.IsNullOrEmpty(this.Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }

            await this.Execute(this.Options.SendGridKey, subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("info@mysite.com", "Password Recovery"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            this.logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}