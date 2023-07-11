using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using SimpleEcommerceV2.Notificator.Domain.Abstractions;
using SimpleEcommerceV2.Notificator.Domain.Models;

namespace SimpleEcommerceV2.Notificator.Domain.Implementations;
  public sealed class AwsSesEmailSender : IEmailSender
    {
        private readonly string _configSetName;
        private readonly ILogger<AwsSesEmailSender> _logger;
        private readonly IAmazonSimpleEmailService _sesClient;

        public AwsSesEmailSender
        (
            ILogger<AwsSesEmailSender> logger,
            IAmazonSimpleEmailService sesClient
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sesClient = sesClient ?? throw new ArgumentNullException(nameof(sesClient));
        }

        public async Task SendAsync
        (
            EmailParams @params,
            CancellationToken cancellationToken
        )
        {
            var sendRequest = new SendTemplatedEmailRequest
            {
                Source = @params.FromAddress,
                Destination = new Destination
                {
                    ToAddresses = @params.ToAddresses?.ToList(),
                    CcAddresses = @params.CcAddresses?.ToList(),
                    BccAddresses = @params.BccAddresses?.ToList()
                },
                Template = @params.TemplateName,
                TemplateData = @params.TemplateData
            };

            try
            {
                var response = await _sesClient.SendTemplatedEmailAsync(sendRequest, cancellationToken);

                _logger.LogInformation
                (
                    "{BeaMailProviderName} {BeaMailProviderStatusCode} {BeaMailStatus} {BeaMailMoreInfo}",
                    "AmazonSesProviderName",
                    response.HttpStatusCode,
                    "EmailSenderConstants.EmailStatus.Sent",
                    "MessageId: " + response.MessageId
                );
            }
            catch (Exception exc)
            {
                //TODO Rename Bea
                _logger.LogError
                (
                    exc,
                    "{BeaMailProviderName} {BeaMailStatus} {BeaMailMoreInfo}",
                    "AmazonSesProviderName",
                    "EmailSenderConstants.EmailStatus.Sent",
                    exc.Message
                );
            }
        }
    }
