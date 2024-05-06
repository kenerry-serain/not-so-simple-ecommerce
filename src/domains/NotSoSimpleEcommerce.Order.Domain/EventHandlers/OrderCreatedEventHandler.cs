using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotSoSimpleEcommerce.Order.Domain.Events;
using NotSoSimpleEcommerce.SesHandler.Models;
using NotSoSimpleEcommerce.Shared.Consts;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;

namespace NotSoSimpleEcommerce.Order.Domain.EventHandlers;

public sealed class OrderCreatedEventHandler: INotificationHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;
    private readonly IMessageSender _messageToEmailQueue;
    private readonly IConfiguration _configuration;

    public OrderCreatedEventHandler
    (
        ILogger<OrderCreatedEventHandler> logger,
        IMessageSender messageEmailQueue, 
        IConfiguration configuration
    )
    {
        _logger = logger ??  throw new ArgumentNullException(nameof( logger));
        _messageToEmailQueue = messageEmailQueue ??  throw new ArgumentNullException(nameof(messageEmailQueue));
        _configuration = configuration??  throw new ArgumentNullException(nameof(configuration));
    }

    public async Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"OrderCreatedEventHandler: Order created by {@event.BoughtBy}");
        // var emailParams = new EmailParams
        // (
        //     _configuration.GetValue<string>("Notificator:EmailConfiguration:From")!,
        //     new List<string> { @event.BoughtBy },
        //     Email.Subjects.OrderCreated,
        //     Email.Templates.OrderCreated,
        //     JsonConvert.SerializeObject(new { Username = @event.BoughtBy })
        // );
        // //TODO ProductStock only on confirmed Order
        // await _messageToEmailQueue.EnqueueAsync(emailParams, cancellationToken);
    }
}
