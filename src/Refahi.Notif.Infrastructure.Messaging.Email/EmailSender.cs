using Refahi.Notif.Domain.Contract.Messaging;
using System.Net;
using System.Net.Mail;

namespace Refahi.Notif.Infrastructure.Messaging.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _config;
        private readonly SemaphoreSlim _rateLimiter; // Semaphore to limit sending rate
        private readonly int _emailsPerMinute = 30; // Maximum emails per minute
        public EmailSender(EmailConfiguration config)
        {
            _config = config;
            _rateLimiter = new SemaphoreSlim(_emailsPerMinute, _emailsPerMinute); // Initialize semaphore
        }
        public async Task Send(string[] addresses, string subject, string body, bool isHtml)
        {
            // Wait to acquire the semaphore
            await _rateLimiter.WaitAsync();

            try
            {
                using var smtpClient = new SmtpClient(_config.SmtpServer, _config.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_config.UserName, _config.Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12;

                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress(_config.From, _config.From);

                foreach (var address in addresses)
                    mail.To.Add(new MailAddress(address));

                mail.Body = body;
                mail.Subject = subject;
                mail.IsBodyHtml = isHtml;

                await smtpClient.SendMailAsync(mail);
            }
            finally
            {
                // Delay to maintain the rate limit (one email per second or 1200ms for safety)
                await Task.Delay(2000); // This will ensure slightly less than 50 emails per minute.
                // Release the semaphore
                _rateLimiter.Release();
            }
        }
    }
}
