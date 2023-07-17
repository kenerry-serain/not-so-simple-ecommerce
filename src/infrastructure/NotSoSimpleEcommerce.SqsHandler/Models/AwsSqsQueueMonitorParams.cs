namespace NotSoSimpleEcommerce.SqsHandler.Models
{
    public record AwsSqsQueueMonitorParams
    (
        string QueueName,
        string QueueOwnerAwsAccountId,
        int MaxNumberOfMessages,
        int WaitTimeSeconds,
        string MsgProcessorServiceName,
        int MillisecondsDelay
    )
    {
        public AwsSqsQueueMonitorParams() : this(string.Empty, string.Empty, int.MinValue, int.MinValue, string.Empty,
            int.MinValue)
        {
        }
    }
}
