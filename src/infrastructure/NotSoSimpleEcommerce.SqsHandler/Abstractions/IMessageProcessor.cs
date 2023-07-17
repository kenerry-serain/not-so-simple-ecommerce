using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.SqsHandler.Abstractions
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(AwsQueueMessageParams awsQueueMessage, CancellationToken cancellationToken);
    }
}
