using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using NotSoSimpleEcommerce.SnsHandler.Abstractions;
using NotSoSimpleEcommerce.SnsHandler.Exceptions;
using NotSoSimpleEcommerce.SnsHandler.Models;

namespace NotSoSimpleEcommerce.SnsHandler.Implementations;

public sealed class AwsSnsMessageSender: IMessageSender
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly AwsSnsMessageParams _snsMessageParams;

    public AwsSnsMessageSender(IAmazonSimpleNotificationService snsClient, AwsSnsMessageParams snsMessageParams)
    {
        _snsClient = snsClient ?? throw new ArgumentNullException(nameof(snsClient));
        _snsMessageParams = snsMessageParams?? throw new ArgumentNullException(nameof(snsMessageParams));
    }
    
    public async Task<string> EnqueueAsync<TObject>(TObject messageBody, CancellationToken cancellationToken)
    {
        var request = new PublishRequest
        {
            TopicArn = _snsMessageParams.TopicArn, 
            Message = JsonConvert.SerializeObject(messageBody)
        };

        var response = await _snsClient.PublishAsync(request, cancellationToken);
        if (response.ResponseMetadata.ChecksumValidationStatus != ChecksumValidationStatus.SUCCESSFUL)
            throw new AwsSnsMessageSenderException("The message is corrupted.");

        return response.MessageId;
    }
}
