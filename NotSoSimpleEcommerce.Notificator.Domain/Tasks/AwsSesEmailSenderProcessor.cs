using Newtonsoft.Json;
using NotSoSimpleEcommerce.MessageHandler.Abstractions;
using NotSoSimpleEcommerce.MessageHandler.Models;
using NotSoSimpleEcommerce.Notificator.Domain.Abstractions;
using NotSoSimpleEcommerce.Notificator.Domain.Models;

namespace NotSoSimpleEcommerce.Notificator.Domain.Tasks
{
    public sealed class AwsSesEmailSenderProcessor : IMessageProcessor
    {
        private readonly IEmailSender _emailSender;
        public AwsSesEmailSenderProcessor
        (
            IEmailSender emailSender
        )
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task ProcessMessageAsync(MessageParams message, CancellationToken cancellationToken)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            
            var emailParams = JsonConvert.DeserializeObject<EmailParams>(message.Body);
            if (emailParams is null)
                throw new ArgumentNullException(nameof(emailParams));
            
            await _emailSender.SendAsync(emailParams, cancellationToken);
        }
    }
}
