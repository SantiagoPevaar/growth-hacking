using Microsoft.AspNetCore.Mvc;
using CustomEmailSender.Models;
using CustomEmailSender.Services;
using SendGrid;

namespace CustomEmailSender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailValidatorController : ControllerBase
    {
        private readonly IEmailValidationService _emailValidationService;
        public EmailValidatorController(IEmailValidationService emailValidationService)
        {
            _emailValidationService = emailValidationService;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateEmail([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be null or empty.");
            }

            try
            {
                var response = await _emailValidationService.ValidateEmail(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
