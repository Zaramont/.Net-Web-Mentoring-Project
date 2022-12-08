using System.Threading.Tasks;

namespace MyCatalogSite.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.CompletedTask;
        }
    }
}