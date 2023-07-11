using MediatR;
using Microsoft.Extensions.Configuration;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.Notificator.Domain.Abstractions;
using SimpleEcommerceV2.Order.Domain.Events;

namespace SimpleEcommerceV2.Order.Domain.EventHandlers;

public sealed class OrderCreatedEventHandler: INotificationHandler<OrderCreatedEvent>
{
    private readonly IMessageSender _messageSender;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public OrderCreatedEventHandler
    (
        IMessageSender messageSender, 
        IEmailSender emailSender,
        IConfiguration configuration
    )
    {
        _messageSender = messageSender ??  throw new ArgumentNullException(nameof(messageSender));
        _emailSender = emailSender??  throw new ArgumentNullException(nameof(emailSender));
        _configuration = configuration??  throw new ArgumentNullException(nameof(configuration));
    }

    public async Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        // TODO Enviar email tb, Remover Default
        // await _emailSender.SendAsync(new EmailParams
        // (
        //     _configuration.GetValue<string>("Notificator:EmailConfiguration:From"),
        //     
        // ));
        await _messageSender.EnqueueAsync(@event, cancellationToken);
    }
}
