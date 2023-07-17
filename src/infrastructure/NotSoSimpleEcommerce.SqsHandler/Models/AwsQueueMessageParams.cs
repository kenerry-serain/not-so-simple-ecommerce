namespace NotSoSimpleEcommerce.SqsHandler.Models
{
    public record AwsQueueMessageParams(string Body, string MessageId);
}
