using MediatR;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.Notificator.Domain.Models;
using SimpleEcommerceV2.Order.Domain.Events;

namespace SimpleEcommerceV2.Order.Domain.EventHandlers;

public sealed class RegisteredOrderEventHandler: INotificationHandler<RegisteredOrderEvent>
{
    private readonly IMessageSender _messageSender;

    public RegisteredOrderEventHandler(IMessageSender messageSender)
    {
        _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
    }

    public async Task Handle(RegisteredOrderEvent notification, CancellationToken cancellationToken)
    {
        var emailParams = new EmailParams
        (
            "kenerry.software.engineer@gmail.com",
            new List<string>{"kenerry13@gmail.com"},
            default,
            default,
            "Subject",
            "Welcome"
        );
        
        await _messageSender.EnqueueAsync(emailParams);
    }
}
