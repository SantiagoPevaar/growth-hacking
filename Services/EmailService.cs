using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using CustomEmailSender.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace CustomEmailSender.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;

        public EmailService(IConfiguration configuration)
        {
            _client = new SendGridClient(configuration["SendGrid:ApiKey"]);
        }

        public async Task SendEmailAsync(EmailCampaign campaign)
        {
            if (string.IsNullOrEmpty(campaign.SenderEmail) || string.IsNullOrEmpty(campaign.RecipientEmail))
            {
                throw new ArgumentException("Sender and recipient emails must be provided.");
            }

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(campaign.SenderEmail, campaign.SenderName));
            msg.SetSubject(campaign.Subject);
            msg.AddTo(new EmailAddress(campaign.RecipientEmail));
            msg.HtmlContent = campaign.HtmlContent ?? string.Empty;
            msg.PlainTextContent = campaign.PlainTextContent ?? string.Empty;

            if (campaign.BBCEmails?.Any() == true)
            {
                msg.AddBccs(campaign.BBCEmails.Select(email => new EmailAddress(email)).ToList());
            }
            msg.AddCategory(campaign.Campaign);
            msg.AddHeader("X-Event-ID", GenerateGuid(campaign.RecipientEmail).ToString());
            msg.SetOpenTracking(true);
            msg.SetClickTracking(true, false);

            var response = await _client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"Error sending email: {response.StatusCode} - {errorBody}");
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
