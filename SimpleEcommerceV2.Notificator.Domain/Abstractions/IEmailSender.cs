using SimpleEcommerceV2.Notificator.Domain.Models;

namespace SimpleEcommerceV2.Notificator.Domain.Abstractions;

public interface IEmailSender
{
    Task SendAsync
    (
        EmailParams @params,
        CancellationToken cancellationToken
    );
}
