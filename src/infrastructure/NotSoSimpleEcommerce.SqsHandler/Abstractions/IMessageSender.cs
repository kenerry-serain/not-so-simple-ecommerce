namespace NotSoSimpleEcommerce.SqsHandler.Abstractions
{
    public interface IMessageSender
    {
        Task<string> EnqueueAsync<TObject>(TObject messageBody, CancellationToken cancellationToken);
    }
}
