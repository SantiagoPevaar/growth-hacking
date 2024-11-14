using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using CustomEmailSender.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace CustomEmailSender.Services
{
    public class EmailValidationService : IEmailValidationService
    {
        private readonly SendGridClient _client;

        public EmailValidationService(IConfiguration configuration)
        {
            _client = new SendGridClient(configuration["SendGrid:EmailValidationApiKey"]);
        }

        public async Task<ValidationResponse> ValidateEmail(string email)
        {
            var data = $@"{{
                ""email"": ""{email}""
            }}";

            var response = await _client.RequestAsync(
                method: SendGridClient.Method.POST, urlPath: "validations/email", requestBody: data);

            var responseBody = await response.Body.ReadAsStringAsync();
            var validationResponse = JsonConvert.DeserializeObject<ValidationResponse>(responseBody);

            return validationResponse;
        }
    }

}
