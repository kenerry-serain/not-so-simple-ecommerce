namespace SimpleEcommerceV2.MessageHandler.Abstractions
{
    public interface IMessageSender
    {
        Task<string> EnqueueAsync<TObject>(TObject messageBody);
    }
}
