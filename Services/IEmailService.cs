using CustomEmailSender.Models;

namespace CustomEmailSender.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailCampaign campaign);
    }
}
