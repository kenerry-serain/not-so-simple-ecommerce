namespace NotSoSimpleEcommerce.MessageHandler.Abstractions
{
    public interface IMessageSender
    {
        Task<string> EnqueueAsync<TObject>(TObject messageBody, CancellationToken cancellationToken);
    }
}
