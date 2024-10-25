namespace CustomEmailSender.Models
{
    public class EmailCampaign
    {
        public string Subject { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string RecipientEmail { get; set; }
        public List<string> BBCEmails { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }
        public string Campaign { get; set; }
    }

}
