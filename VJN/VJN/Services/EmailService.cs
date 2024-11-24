using Microsoft.Extensions.Options;
using System.Net.Mail;
using VJN.ModelsDTO.EmailDTOs;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Net.Smtp;

namespace VJN.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.From, _emailSettings.From));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    await smtp.SendAsync(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Gặp lỗi khi gửi email: {ex.Message}");
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }

        public async Task SendEmailAsyncWithHTML(string recipientEmail, string subject, string htmlContent)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.From, _emailSettings.From));
            email.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlContent
            };
            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Timeout = 10000; // 10 giây
                    await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    await smtp.SendAsync(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Gặp lỗi khi gửi email: {ex.Message}\n{ex.StackTrace}");
                }
                finally
                {
                    if (smtp.IsConnected)
                    {
                        await smtp.DisconnectAsync(true);
                    }
                }
            }
        }
    }
}
