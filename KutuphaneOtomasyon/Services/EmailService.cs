    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
    using NETCore.MailKit.Core;
    using NETCore.MailKit.Infrastructure.Internal;

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "in-v3.mailjet.com";
        private readonly int _smtpPort = 587;
        private readonly string _emailSender = "5c079f740e46fe0e62891fe27ff8c875";
        private readonly string _emailPassword = "f47708d4feef81aae81d39c877985b63";

        public async Task SendAsync(string toEmail, string subject, string message, bool isHtml = true)
        {
            throw new Exception();
        }

        void IEmailService.Send(string mailTo, string subject, string message, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string subject, string message, string[] attachments, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string subject, string message, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string subject, string message, string[] attachments, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string mailCc, string mailBcc, string subject, string message, string[] attachments, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        void IEmailService.Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, string[] attachments, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        async Task IEmailService.SendAsync(string mailTo, string subject, string message, bool isHtml, SenderInfo sender)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_emailSender, _emailPassword);
                client.EnableSsl = false; // Güvenli bağlantı

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mailTo),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isHtml
                };

                mailMessage.To.Add(mailTo);

                await client.SendMailAsync(mailMessage);
            }

        }

        Task IEmailService.SendAsync(string mailTo, string subject, string message, string[] attachments, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        Task IEmailService.SendAsync(string mailTo, string subject, string message, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        Task IEmailService.SendAsync(string mailTo, string subject, string message, string[] attachments, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        Task IEmailService.SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        Task IEmailService.SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, string[] attachments, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        Task IEmailService.SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }

        Task IEmailService.SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, string[] attachments, Encoding encoding, bool isHtml, SenderInfo sender)
        {
            throw new NotImplementedException();
        }
    }
