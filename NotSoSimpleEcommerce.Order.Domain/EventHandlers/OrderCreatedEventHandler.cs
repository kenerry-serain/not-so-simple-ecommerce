using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NotSoSimpleEcommerce.MessageHandler.Abstractions;
using NotSoSimpleEcommerce.Notificator.Domain.Models;
using NotSoSimpleEcommerce.Order.Domain.Events;
using NotSoSimpleEcommerce.Shared.Consts;

namespace NotSoSimpleEcommerce.Order.Domain.EventHandlers;

public sealed class OrderCreatedEventHandler: INotificationHandler<OrderCreatedEvent>
{
    private readonly IMessageSender _messageToEmailQueue;
    private readonly IConfiguration _configuration;

    public OrderCreatedEventHandler
    (
        IMessageSender messageEmailQueue, 
        IConfiguration configuration
    )
    {
        _messageToEmailQueue = messageEmailQueue ??  throw new ArgumentNullException(nameof(messageEmailQueue));
        _configuration = configuration??  throw new ArgumentNullException(nameof(configuration));
    }

    public async Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        var emailParams = new EmailParams
        (
            _configuration.GetValue<string>("Notificator:EmailConfiguration:From")!,
            new List<string> { @event.BoughtBy },
            Email.Subjects.OrderCreated,
            Email.Templates.OrderCreated,
            JsonConvert.SerializeObject(new { Username = @event.BoughtBy })
        );
        //TODO ProductStock only on confirmed Order
        await _messageToEmailQueue.EnqueueAsync(emailParams, cancellationToken);
        // await _messageProductStockQueue.EnqueueAsync(@event, cancellationToken);
    }
}
