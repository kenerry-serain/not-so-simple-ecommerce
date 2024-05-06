using Newtonsoft.Json;
using NotSoSimpleEcommerce.SesHandler.Models;
using NotSoSimpleEcommerce.Shared.Consts;
using NotSoSimpleEcommerce.Shared.Events;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.InvoiceGenerator.Tasks;

public class InvoiceProcessor : IMessageProcessor
{
    private readonly IMessageSender _messageEmailQueue;
    private readonly IConfiguration _configuration;

    public InvoiceProcessor(IMessageSender messageEmailQueue, IConfiguration configuration)
    {
        _messageEmailQueue = messageEmailQueue;
        _configuration = configuration;
    }

    public async Task ProcessMessageAsync(AwsQueueMessageParams awsQueueMessage, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Invoice received: {awsQueueMessage}");
        var sqsEvent = JsonConvert.DeserializeObject<dynamic>(awsQueueMessage.Body);
        if (sqsEvent is null)
            throw new ArgumentNullException(nameof(sqsEvent));

        var orderConfirmedEvent = JsonConvert.DeserializeObject<OrderConfirmedEvent>(sqsEvent.Message.ToString());
        if (orderConfirmedEvent is null)
            throw new ArgumentNullException(nameof(sqsEvent));

        var emailParams = new EmailParams
        (
            _configuration.GetValue<string>("Notificator:EmailConfiguration:From")!,
            new List<string> () {_configuration.GetValue<string>("Notificator:EmailConfiguration:To")!},
            Email.Subjects.OrderCreated,
            _configuration.GetValue<string>("Notificator:EmailConfiguration:SesTemplate")!,
            JsonConvert.SerializeObject(new { OrderId = orderConfirmedEvent.Id, InvoiceNumber=new Random().Next(1000,2000) })
        );

        await _messageEmailQueue.EnqueueAsync(emailParams, cancellationToken);
        Console.WriteLine($"Invoice Generated for orderId {orderConfirmedEvent.Id}!");
    }
}
