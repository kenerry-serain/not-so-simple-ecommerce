namespace SimpleEcommerceV2.MessageHandler.Models
{
    public class AwsSqsMessageParams
    {
        public AwsSqsMessageParams(){}
        public AwsSqsMessageParams(bool isFifo, bool enableMessageDeduplication, string queueName, string queueOwnerAwsAccountId)
        {
            IsFifo = isFifo;
            EnableMessageDeduplication = enableMessageDeduplication;
            QueueName = queueName;
            QueueOwnerAwsAccountId = queueOwnerAwsAccountId;
        }

        public bool IsFifo { get; set; }
        public bool EnableMessageDeduplication{ get; set; }
        public string QueueName { get; set; }
        public string QueueOwnerAwsAccountId { get; set; }
    }
}
