namespace SimpleEcommerceV2.MessageHandler.Models
{
    public class QueueMessageParams
    {
        public QueueMessageParams(string body, string messageId)
        {
            Body = body;
            MessageId = messageId;
        }

        public string Body { get; set; }
        public string MessageId { get; set; }
    }
}
