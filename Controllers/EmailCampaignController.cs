using Microsoft.AspNetCore.Mvc;
using CustomEmailSender.Models;
using CustomEmailSender.Services;
using SendGrid;

namespace CustomEmailSender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailCampaignController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailCampaignController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendCampaign([FromBody] EmailCampaign campaign)
        {
            if (campaign == null || string.IsNullOrEmpty(campaign.RecipientEmail))
            {
                return BadRequest("Invalid email campaign details.");
            }

            try
            {
                await _emailService.SendEmailAsync(campaign);
                return Ok("Email campaign sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
