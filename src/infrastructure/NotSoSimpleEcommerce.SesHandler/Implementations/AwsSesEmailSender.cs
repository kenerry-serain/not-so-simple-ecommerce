using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using NotSoSimpleEcommerce.SesHandler.Abstractions;
using NotSoSimpleEcommerce.SesHandler.Models;

namespace NotSoSimpleEcommerce.SesHandler.Implementations;
  public sealed class AwsSesEmailSender : IEmailSender
    {
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
                    ToAddresses = @params.ToAddresses.ToList(),
                    CcAddresses = @params.CcAddresses?.ToList() ?? new (),
                    BccAddresses = @params.BccAddresses?.ToList()?? new ()
                },
                Template = @params.TemplateName,
                TemplateData = @params.TemplateData
            };

            try
            {
                var response = await _sesClient.SendTemplatedEmailAsync(sendRequest, cancellationToken);

                _logger.LogInformation
                (
                    "Email sent: {StatusCode}, Response {MailMoreInfo}",
                    response.HttpStatusCode,
                    response.ToString()
                );
            }
            catch (Exception exception)
            {
                _logger.LogError
                (
                    exception,
                    "An error happened while sending an email: {MailMoreInfo}",
                    exception.ToString()
                );
            }
        }
    }
