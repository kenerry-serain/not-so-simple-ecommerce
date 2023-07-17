using Newtonsoft.Json;
using NotSoSimpleEcommerce.Shared.Events;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.InvoiceGenerator.Tasks;

public class InvoiceProcessor: IMessageProcessor
{
    public Task ProcessMessageAsync(AwsQueueMessageParams awsQueueMessage, CancellationToken cancellationToken)
    {
        var orderConfirmedEvent = JsonConvert.DeserializeObject<OrderConfirmedEvent>(awsQueueMessage.Body);
        if (orderConfirmedEvent is null)
            throw new ArgumentNullException(nameof(orderConfirmedEvent));
        
        Console.WriteLine($"Invoice Generated for orderId {orderConfirmedEvent.Id}!");
        return Task.CompletedTask;
    }
}
