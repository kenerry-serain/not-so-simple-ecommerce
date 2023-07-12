using NotSoSimpleEcommerce.MessageHandler.Models;

namespace NotSoSimpleEcommerce.MessageHandler.Abstractions
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(MessageParams message, CancellationToken cancellationToken);
    }
}
