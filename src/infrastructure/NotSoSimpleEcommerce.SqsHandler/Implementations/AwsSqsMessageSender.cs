using System.Text;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Exceptions;
using NotSoSimpleEcommerce.SqsHandler.Models;
using NotSoSimpleEcommerce.Utils.Encryption;

namespace NotSoSimpleEcommerce.SqsHandler.Implementations
{
    public sealed class AwsSqsMessageSender : IMessageSender
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly ILogger<AwsSqsMessageSender> _logger;
        private readonly AwsSqsMessageSenderParams _senderSenderParams;
        private readonly IMemoryCache _memoryCache;

        public AwsSqsMessageSender
        (
            IAmazonSQS sqsClient,
            ILogger<AwsSqsMessageSender> logger,
            AwsSqsMessageSenderParams senderSenderParams,
            IMemoryCache memoryCache
        )
        {
            _sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
            _senderSenderParams = senderSenderParams ?? throw new ArgumentNullException(nameof(senderSenderParams));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<string> EnqueueAsync<TObject>
        (
            TObject messageBody,
            CancellationToken cancellationToken
        )
        {
            var queueUrl = await GetQueueUrlAsync(_sqsClient);
            var serializedMessageBody = JsonConvert.SerializeObject(messageBody);
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = serializedMessageBody
            };

            if (_senderSenderParams.IsFifo)
            {
                sendMessageRequest.MessageGroupId = Guid.NewGuid().ToString();
                sendMessageRequest.MessageDeduplicationId = _senderSenderParams.EnableMessageDeduplication ? 
                    Hasher.GetMd5(Encoding.UTF8.GetBytes(serializedMessageBody)) : 
                    Guid.NewGuid().ToString();
            }

            var response = await _sqsClient.SendMessageAsync(sendMessageRequest, cancellationToken);
            var messageMd5 = Hasher.GetMd5(Encoding.UTF8.GetBytes(serializedMessageBody));
            var responseMd5 = response.MD5OfMessageBody;
            if (messageMd5 != responseMd5)
                throw new AwsSqsMessageSenderException("The message is corrupted.");

            _logger.LogInformation
            (
                "Message sent {MessageId} to Queue {QueueName}", 
                response.MessageId,
                _senderSenderParams.QueueName
            );

            return response.MessageId;
        }

        private async Task<string> GetQueueUrlAsync(IAmazonSQS sqsClient)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"{_senderSenderParams.QueueName}_{_senderSenderParams.QueueOwnerAwsAccountId}", 
                async cacheManager =>
            {
                cacheManager.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
                var queueUrlRequest = new GetQueueUrlRequest
                {
                    QueueName = _senderSenderParams.QueueName,
                    QueueOwnerAWSAccountId = _senderSenderParams.QueueOwnerAwsAccountId
                };
                var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueUrlRequest);
                return queueUrlResponse.QueueUrl;
            }) ?? throw new KeyNotFoundException("The specified queue doesn't exists.");
        }
    }
}
