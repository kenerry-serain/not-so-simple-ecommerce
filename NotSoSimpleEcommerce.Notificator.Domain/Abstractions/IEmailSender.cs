using NotSoSimpleEcommerce.Notificator.Domain.Models;

namespace NotSoSimpleEcommerce.Notificator.Domain.Abstractions;

public interface IEmailSender
{
    Task SendAsync
    (
        EmailParams @params,
        CancellationToken cancellationToken
    );
}
