using Newtonsoft.Json;

namespace CustomEmailSender.Models
{
    public class WebHookResponse
    {
        [JsonProperty("id")]
        public string? id => $"{Email}-{Event}-{EventTime.ToString("yyyy-MM-ddTHH:mm:ss")}";

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("datetime")]
        public DateTime EventTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;    
        [JsonProperty("smtp-id")]
        public string? SmtpId { get; set; }
        [JsonProperty("sg_event_id")]
        public string? SgEventId { get; set; }
        [JsonProperty("sg_message_id")]
        public string? SgMessageId { get; set; }
        [JsonProperty("response")]
        public string? Response { get; set; }
        [JsonProperty("attempt")]
        public int? Attempt { get; set; }
        [JsonProperty("category")]
        public List<string> Category { get; set; }
        [JsonProperty("url")]
        public string? Url { get; set; }
        [JsonProperty("useragent")]
        public string? UserAgent { get; set; }
        [JsonProperty("ip")]
        public string? Ip { get; set; }
        [JsonProperty("reason")]
        public string? Reason { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("bounce_classification")]
        public string? BounceType { get; set; }
        [JsonProperty("asm_group_id")]
        public string? AsmGroupId { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("sg_machine_open")]
        public string? SgMachineOpen { get; set; }
        [JsonProperty("marketing_campaign_id")]
        public string? MarketingCampaignId { get; set; }
        [JsonProperty("marketing_campaign_name")]
        public string? MarketingCampaignName { get; set; }
      
        public WebHookResponse()
        {
            Category = new List<string>();
        }
    }

}
