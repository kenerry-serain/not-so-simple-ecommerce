using NotSoSimpleEcommerce.SesHandler.Models;

namespace NotSoSimpleEcommerce.SesHandler.Abstractions;

public interface IEmailSender
{
    Task SendAsync
    (
        EmailParams @params,
        CancellationToken cancellationToken
    );
}
