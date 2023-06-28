using Newtonsoft.Json;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.MessageHandler.Models;
using SimpleEcommerceV2.Notificator.Domain.Abstractions;
using SimpleEcommerceV2.Notificator.Domain.Models;

namespace SimpleEcommerceV2.Notificator.Domain.Tasks
{
    public sealed class AwsSesEmailSenderProcessor : IMessageProcessor
    {
        private readonly IEmailSender _emailSender;
        public AwsSesEmailSenderProcessor(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task ProcessMessageAsync(QueueMessageParams message, CancellationToken cancellationToken)
        {
            var emailParams = JsonConvert.DeserializeObject<EmailParams>(message.Body);
            if (emailParams is null)
                throw new ArgumentNullException(nameof(emailParams));
            
            await _emailSender.SendAsync(emailParams, cancellationToken);
        }
    }
}
