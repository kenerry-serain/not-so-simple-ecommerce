namespace NotSoSimpleEcommerce.SqsHandler.Models
{
    public record AwsQueueMessageParams(string Body, string MessageId);
    public class SnsEnvelope
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
    }
}
