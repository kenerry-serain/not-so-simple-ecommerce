using System.Text;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleEcommerceV2.MessageHandler.Abstractions;
using SimpleEcommerceV2.MessageHandler.Exceptions;
using SimpleEcommerceV2.MessageHandler.Models;
using SimpleEcommerceV2.MessageHandler.Utils;

namespace SimpleEcommerceV2.MessageHandler.Implementations
{
    public sealed class AwsSqsMessageSender : IMessageSender
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly ILogger<AwsSqsMessageSender> _logger;
        private readonly AwsSqsMessageParams _senderParams;
        private readonly IMemoryCache _memoryCache;

        public AwsSqsMessageSender
        (
            IAmazonSQS sqsClient,
            ILogger<AwsSqsMessageSender> logger,
            AwsSqsMessageParams senderParams,
            IMemoryCache memoryCache
        )
        {
            _sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
            _senderParams = senderParams ?? throw new ArgumentNullException(nameof(senderParams));
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

            if (_senderParams.IsFifo)
            {
                sendMessageRequest.MessageGroupId = Guid.NewGuid().ToString();
                sendMessageRequest.MessageDeduplicationId = _senderParams.EnableMessageDeduplication ? 
                    Hasher.GetMd5(Encoding.UTF8.GetBytes(serializedMessageBody)) : 
                    Guid.NewGuid().ToString();
            }

            var response = await _sqsClient.SendMessageAsync(sendMessageRequest);
            var messageMd5 = Hasher.GetMd5(Encoding.UTF8.GetBytes(serializedMessageBody));
            var responseMd5 = response.MD5OfMessageBody;
            if (messageMd5 != responseMd5)
                throw new AwsSqsMessageSenderException("The message is corrupted.");

            _logger.LogInformation($"Message sent {response.MessageId} to Queue {_senderParams.QueueName}");

            return response.MessageId;
        }

        private async Task<string> GetQueueUrlAsync(IAmazonSQS sqsClient)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"{_senderParams.QueueName}_{_senderParams.QueueOwnerAwsAccountId}", 
                async (cacheManager) =>
            {
                cacheManager.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
                var queueUrlRequest = new GetQueueUrlRequest
                {
                    QueueName = _senderParams.QueueName,
                    QueueOwnerAWSAccountId = _senderParams.QueueOwnerAwsAccountId
                };
                var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueUrlRequest);
                return queueUrlResponse.QueueUrl;
            }) ?? throw new KeyNotFoundException("The specified queue doesn't exists.");
        }
    }
}
