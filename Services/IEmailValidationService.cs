using CustomEmailSender.Models;

namespace CustomEmailSender.Services
{
    public interface IEmailValidationService
    {
        Task<ValidationResponse> ValidateEmail(string email);
    }
}
