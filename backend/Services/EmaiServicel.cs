using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace whm.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendVerificationCodeAsync(
            string email,
            string code)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    "Stock Management System",
                    configuration["EmailSettings:Username"]!
                ));

            message.To.Add(
                MailboxAddress.Parse(email)
            );

            message.Subject = "Email Verification Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your verification code is: {code}\n\n" +
                       "This code will expire in 10 minutes."
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                configuration["EmailSettings:Host"],
                int.Parse(configuration["EmailSettings:Port"]!),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                configuration["EmailSettings:Username"],
                configuration["EmailSettings:Password"]
            );

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}