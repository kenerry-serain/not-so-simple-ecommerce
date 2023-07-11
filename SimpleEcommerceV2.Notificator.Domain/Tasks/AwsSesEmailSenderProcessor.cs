using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public AwsSesEmailSenderProcessor
        (
            IEmailSender emailSender,
            IConfiguration configuration
        )
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task ProcessMessageAsync(MessageParams message, CancellationToken cancellationToken)
        {
            //TODO add on settings / Iac Named IMessageHandler, Iac Named Sns Params
            var emailParams = JsonConvert.DeserializeObject<EmailParams>(message.Body) ?? new EmailParams
            (
                _configuration.GetValue<string>("Notificator:EmailConfiguration:From")!,
                new List<string> { "kenerry13@gmail.com" },
                default,
                default,
                "Subject",
                "Welcome",
                JsonConvert.SerializeObject(new { username = "Admin" })
            );

            await _emailSender.SendAsync(emailParams, cancellationToken);
        }
    }
}
