namespace SimpleEcommerceV2.MessageHandler.Models
{
    public class MessageParams
    {
        public MessageParams(string body, string messageId)
        {
            Body = body;
            MessageId = messageId;
        }

        public string Body { get; set; }
        public string MessageId { get; set; }
    }
}
