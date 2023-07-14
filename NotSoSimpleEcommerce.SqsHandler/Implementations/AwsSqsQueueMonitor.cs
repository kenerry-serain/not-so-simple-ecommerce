using Amazon.SQS;
using Amazon.SQS.Model;
using Autofac;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.SqsHandler.Implementations
{
    public class AwsSqsQueueMonitor : BackgroundService
    {
        private readonly ILifetimeScope _container;
        private readonly ILogger<AwsSqsQueueMonitor> _logger;
        private readonly AwsSqsQueueMonitorParams _monitorParams;
        private readonly IMemoryCache _memoryCache;
        private string _queueUrl = string.Empty;

        public AwsSqsQueueMonitor
        (
            ILifetimeScope container,
            ILogger<AwsSqsQueueMonitor> logger,
            AwsSqsQueueMonitorParams monitorParams,
            IMemoryCache memoryCache
        )
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _monitorParams = monitorParams ?? throw new ArgumentNullException(nameof(monitorParams));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queue Monitor initialized");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_monitorParams.MillisecondsDelay, cancellationToken);
                    await CheckQueueAsync(cancellationToken);
                }
                catch (OperationCanceledException exception)
                {
                    _logger.LogWarning(exception, "Monitor stopped");
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error verifying queue");
                }
            }
        }

        private async Task CheckQueueAsync(CancellationToken cancellationToken)
        {
            await using var scope = _container.BeginLifetimeScope();
            var sqsClient = scope.ResolveNamed<IAmazonSQS>(nameof(IAmazonSQS));
            if (string.IsNullOrWhiteSpace(_queueUrl))
            {
                _queueUrl = await GetQueueUrlAsync(sqsClient);
                _logger.LogInformation("Queue {QueueName} url {QueueUrl}",
                    _monitorParams.QueueName, _queueUrl);
            }

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                VisibilityTimeout = 300,
                MaxNumberOfMessages = _monitorParams.MaxNumberOfMessages,
                WaitTimeSeconds = _monitorParams.WaitTimeSeconds
            };

            var receiveMessageResponse =
                await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cancellationToken);

            foreach (var message in receiveMessageResponse.Messages)
                await ProcessMessageAsync(sqsClient, message, cancellationToken);
        }

        private async Task ProcessMessageAsync(IAmazonSQS sqsClient, Message message, CancellationToken cancellationToken)
        {
            try
            {
                await using var scope = _container.BeginLifetimeScope("ProcessMessageScope");
                var messageProcessor = scope.ResolveNamed<IMessageProcessor>
                (
                    _monitorParams.MsgProcessorServiceName
                );

                _logger.LogInformation("{QueueName} received {MessageId}",
                    _monitorParams.QueueName, message.MessageId);

                var messageParams = new AwsQueueMessageParams(message.Body, message.MessageId);
                var start = DateTime.Now;
                
                _logger.LogInformation($"{start.ToLongTimeString()}");

                await messageProcessor.ProcessMessageAsync(messageParams, cancellationToken);

                _logger.LogInformation("{QueueName} processed {MessageId}",
                    _monitorParams.QueueName, message.MessageId);

                _logger.LogInformation($"{DateTime.Now.Subtract(start).TotalMilliseconds}");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{QueueName} fail {MessageId}",
                    _monitorParams.QueueName, message.MessageId);
            }
            finally
            {
                await DeleteMessageAsync(sqsClient, message, cancellationToken);

                _logger.LogInformation("{QueueName} removed {MessageId}",
                    _monitorParams.QueueName, message.MessageId);
            }
        }
        
        private async Task<string> GetQueueUrlAsync(IAmazonSQS sqsClient)
        {
            return (await _memoryCache.GetOrCreateAsync($"{_monitorParams.QueueName}_{_monitorParams.QueueOwnerAwsAccountId}", async cacheManager =>
            {
                cacheManager.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                var queueUrlRequest = new GetQueueUrlRequest
                {
                    QueueName = _monitorParams.QueueName,
                    QueueOwnerAWSAccountId = _monitorParams.QueueOwnerAwsAccountId
                };
                var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueUrlRequest);
                return queueUrlResponse.QueueUrl;
            })) ?? throw new KeyNotFoundException("The specified queue does not exists.");
        }

        private async Task DeleteMessageAsync(IAmazonSQS sqsClient, Message message, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Going to remove queue message from {QueueName}", _queueUrl);

                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = _queueUrl,
                    ReceiptHandle = message.ReceiptHandle
                };
                await sqsClient.DeleteMessageAsync(deleteMessageRequest, cancellationToken);
            }
            catch (Exception)
            {
                _logger.LogCritical("Error on deleting queue message {QueueName}",_queueUrl);
            }
        }
    }
}
