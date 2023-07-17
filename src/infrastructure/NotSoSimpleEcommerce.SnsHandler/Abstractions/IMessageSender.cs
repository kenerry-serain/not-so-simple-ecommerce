namespace NotSoSimpleEcommerce.SnsHandler.Abstractions
{
    public interface IMessageSender
    {
        Task<string> EnqueueAsync<TObject>(TObject messageBody, CancellationToken cancellationToken);
    }
}
