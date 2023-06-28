namespace SimpleEcommerceV2.MessageHandler.Models
{
    public class AwsSqsQueueMonitorParams
    {
        public AwsSqsQueueMonitorParams() { }
        public AwsSqsQueueMonitorParams
        (
            string queueName, 
            string queueOwnerAwsAccountId, 
            int maxNumberOfMessages, 
            int waitTimeSeconds, 
            string awsSqsServiceName, 
            string msgProcessorServiceName, 
            int millisecondsDelay
        )
        {
            QueueName = queueName;
            QueueOwnerAwsAccountId = queueOwnerAwsAccountId;
            MaxNumberOfMessages = maxNumberOfMessages;
            WaitTimeSeconds = waitTimeSeconds;
            MsgProcessorServiceName = msgProcessorServiceName;
            MillisecondsDelay = millisecondsDelay;
        }

        public string QueueName { get; set; }
        public string QueueOwnerAwsAccountId { get; set; }
        public int MaxNumberOfMessages { get; set; }
        public int WaitTimeSeconds { get; set; } = 0;
        public string MsgProcessorServiceName { get; set; }
        public int MillisecondsDelay { get; set; }
    }
}
