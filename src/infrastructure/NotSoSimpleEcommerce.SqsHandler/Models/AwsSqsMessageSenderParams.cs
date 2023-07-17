namespace NotSoSimpleEcommerce.SqsHandler.Models
{
    public record AwsSqsMessageSenderParams
    (
        bool IsFifo,
        bool EnableMessageDeduplication,
        string QueueName,
        string QueueOwnerAwsAccountId
    )
    {
        public AwsSqsMessageSenderParams() : this(false, false, string.Empty, String.Empty) { }
    }
}
