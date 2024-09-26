using IForgotMyPassword.Abstraction;
using System.Net.Mail;
using System.Net;

namespace IForgotMyPassword.Concrete
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            mail.To.Add(to);
            mail.From = new(_configuration["Mail:UserName"], "DemoProject", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Host = _configuration["Mail:Host"];
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
            await smtp.SendMailAsync(mail);
        }
    }
}
