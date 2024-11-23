namespace VJN.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string body);

        public Task SendEmailAsyncWithHTML(string recipientEmail, string subject, string htmlContent);
    }
}
