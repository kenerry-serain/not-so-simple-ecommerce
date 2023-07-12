using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using NotSoSimpleEcommerce.MessageHandler.Abstractions;
using NotSoSimpleEcommerce.MessageHandler.Exceptions;
using NotSoSimpleEcommerce.MessageHandler.Models;

namespace NotSoSimpleEcommerce.MessageHandler.Implementations;

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
        var statusCode = (int)response.HttpStatusCode;
        if (statusCode is >= 200 and <= 299)
            throw new AwsSnsMessageSenderException("The message is corrupted.");
        
        return response.MessageId;
    }
}
