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

        public string GetEmailHTML(string header, string h2body, string h2p)
        {
            string htmlContent = @$"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Eco Green Email Template</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;600&display=swap');

        body {{
            font-family: 'Montserrat', Arial, sans-serif;
            line-height: 1.6;
            background-color: #e6f3e6;
            margin: 0;
            padding: 0;
            color: #2c3e50;
        }}

        .email-container {{
            max-width: 600px;
            margin: 30px auto;
            background-color: white;
            border-radius: 15px;
            box-shadow: 0 12px 30px rgba(0,0,0,0.08);
            overflow: hidden;
            border-top: 6px solid #2ecc71;
        }}

        .email-header {{
            background: linear-gradient(135deg, #2ecc71, #27ae60);
            color: white;
            text-align: center;
            padding: 25px;
            position: relative;
        }}

        .email-header h1 {{
            margin: 0;
            font-weight: 600;
            font-size: 26px;
            letter-spacing: 1px;
            text-shadow: 1px 1px 2px rgba(0,0,0,0.2);
        }}

        .email-body {{
            padding: 30px;
            background-color: #f9fff9;
        }}

        .email-body h2 {{
            color: #27ae60;
            margin-bottom: 15px;
            font-weight: 600;
            border-bottom: 2px solid #2ecc71;
            padding-bottom: 10px;
        }}

        .email-body p {{
            color: #34495e;
            margin-bottom: 20px;
        }}

        .btn {{
            display: inline-block;
            background: linear-gradient(135deg, #2ecc71, #27ae60);
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 30px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
            transition: all 0.3s ease;
            box-shadow: 0 5px 15px rgba(46,204,113,0.3);
        }}

        .btn:hover {{
            transform: translateY(-3px);
            box-shadow: 0 7px 20px rgba(46,204,113,0.4);
            background: linear-gradient(135deg, #27ae60, #2ecc71);
        }}

        .feature-list {{
            background-color: #f0f9f0;
            padding: 20px;
            border-radius: 10px;
            margin-top: 20px;
        }}

        .feature-list ul {{
            list-style-type: none;
            padding: 0;
        }}

        .feature-list ul li {{
            padding: 8px 0;
            border-bottom: 1px solid #dff0df;
            color: #2c3e50;
        }}

        .feature-list ul li:last-child {{
            border-bottom: none;
        }}

        .email-footer {{
            background-color: #e6f3e6;
            text-align: center;
            padding: 15px;
            font-size: 12px;
            color: #7f8c8d;
        }}

        .social-links {{
            margin-top: 20px;
            display: flex;
            justify-content: center;
            gap: 20px;
        }}

        .social-links a {{
            color: #2ecc71;
            text-decoration: none;
            font-size: 16px;
            transition: color 0.3s ease;
        }}

        .social-links a:hover {{
            color: #27ae60;
        }}

        @media only screen and (max-width: 600px) {{
            .email-container {{
                width: 100%;
                margin: 0;
                border-radius: 0;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""email-header"">
            <h1>{header}</h1>
        </div>
        
        <div class=""email-body"">
            <h2>{h2body}</h2>
            <p>{h2p}</p>
        </div>
        
        <div class=""email-footer"">
            <p>Việc Nhanh - Nền tảng tuyển dụng và tìm việc bán thời gian trực tuyến</p>
        </div>
    </div>
</body>
</html>";
            return htmlContent;
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
