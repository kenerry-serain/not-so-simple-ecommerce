using SimpleEcommerceV2.MessageHandler.Models;

namespace SimpleEcommerceV2.MessageHandler.Abstractions
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(MessageParams message, CancellationToken cancellationToken);
    }
}
