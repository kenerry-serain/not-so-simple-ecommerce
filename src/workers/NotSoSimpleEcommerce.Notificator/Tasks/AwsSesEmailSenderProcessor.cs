using Newtonsoft.Json;
using NotSoSimpleEcommerce.SesHandler.Abstractions;
using NotSoSimpleEcommerce.SesHandler.Models;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.Notificator.Tasks
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

        public async Task ProcessMessageAsync(AwsQueueMessageParams awsQueueMessage, CancellationToken cancellationToken)
        {
            if (awsQueueMessage is null)
                throw new ArgumentNullException(nameof(awsQueueMessage));
            
            var emailParams = JsonConvert.DeserializeObject<EmailParams>(awsQueueMessage.Body);
            if (emailParams is null)
                throw new ArgumentNullException(nameof(emailParams));
            
            await _emailSender.SendAsync(emailParams, cancellationToken);
        }
    }
}
