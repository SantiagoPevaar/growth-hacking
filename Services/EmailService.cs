using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using CustomEmailSender.Models;
using System.Security.Cryptography;
using System.Text;

namespace CustomEmailSender.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailCampaign campaign)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(campaign.SenderEmail, campaign.SenderName);
            var subject = campaign.Subject;
            var to = new EmailAddress(campaign.RecipientEmail);
            var plainTextContent = campaign.PlainTextContent;
            var htmlContent = campaign.HtmlContent;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            foreach (var bccEmail in campaign.BBCEmails)
            {
                var bcc = new EmailAddress(bccEmail);
                msg.AddBcc(bcc);
            }
            msg.AddHeader("X-Event-ID", GenerateGuid(campaign.RecipientEmail).ToString());
            msg.SetOpenTracking(true);
            msg.SetClickTracking(true, false);

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error sending email: {response.StatusCode}");
            }
        }

        private Guid GenerateGuid(string url)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
                return new Guid(hashBytes);
            }
        }
    }

}
