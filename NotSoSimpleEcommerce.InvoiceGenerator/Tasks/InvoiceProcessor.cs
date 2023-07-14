using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.InvoiceGenerator.Tasks;

public class InvoiceProcessor: IMessageProcessor
{
    public Task ProcessMessageAsync(AwsQueueMessageParams awsQueueMessage, CancellationToken cancellationToken)
    {
        //TODO Receive the order, Email?
        Console.WriteLine("Invoice Generated!");
        return Task.CompletedTask;
    }
}
