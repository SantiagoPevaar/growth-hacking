using Azure.Core;
using CustomEmailSender.Models;
using CustomEmailSender.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomEmailSender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailEventController : ControllerBase
    {
        private readonly ICosmosService _cosmosService;

        public EmailEventController(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        [HttpPost]
        [Route("receive")]
        public async Task<IActionResult> ReceiveWebHookAsync()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                List<WebHookResponse> webhookResponses;

                try
                {
                    webhookResponses = JsonConvert.DeserializeObject<List<WebHookResponse>>(body);
                }
                catch (JsonException)
                {
                    return BadRequest("Invalid JSON format");
                }

                foreach (var response in webhookResponses)
                {
                    try
                    {
                        if (!response.Email.Contains("pevaar.com") && response.Category.Count != 0)
                        {
                            await _cosmosService.AddItemAsync(response); 
                        }
                    }
                    catch (Exception){ }
                }

                return Ok("Webhook received successfully");
            }
        }

    }
}
