using Newtonsoft.Json;
using NotSoSimpleEcommerce.Shared.Events;
using NotSoSimpleEcommerce.Shared.HttpHandlers.Contracts;
using NotSoSimpleEcommerce.SqsHandler.Abstractions;
using NotSoSimpleEcommerce.SqsHandler.Models;

namespace NotSoSimpleEcommerce.Main.Domain.Tasks;

public sealed class ProductStockProcessor: IMessageProcessor
{
    private readonly IMainApi _mainApiClient;
    private readonly IOrderApi _orderApiClient;

    public ProductStockProcessor
    (
        IMainApi mainApiClient,
        IOrderApi orderApiClient
    )
    {
        _mainApiClient = mainApiClient ?? throw new ArgumentNullException(nameof(mainApiClient));
        _orderApiClient = orderApiClient?? throw new ArgumentNullException(nameof(orderApiClient));
    }

    public async Task ProcessMessageAsync(AwsQueueMessageParams awsQueueMessage, CancellationToken cancellationToken)
    {
        var orderCreatedEvent = JsonConvert.DeserializeObject<OrderConfirmedEvent>(awsQueueMessage.Body);
        if (orderCreatedEvent is null)
            throw new ArgumentNullException(nameof(orderCreatedEvent));

        var orderApiResponse = await _orderApiClient.GetOrderByIdAsync(orderCreatedEvent.Id);
        var orderResponse = orderApiResponse.Content ?? throw new ArgumentNullException();
        if (!orderApiResponse.IsSuccessStatusCode)
            throw new KeyNotFoundException("Sorry, something went wrong while consulting the order.");
        
        var stockApiResponse = await _mainApiClient.GetStockByProductIdAsync(orderResponse.Product.Id);
        var stockResponse = stockApiResponse.Content?? throw new ArgumentNullException();
        if (!stockApiResponse.IsSuccessStatusCode)
            throw new KeyNotFoundException("Sorry, something went wrong while consulting the product stock.");

        var response = await _mainApiClient.UpdateProductStockAsync(orderResponse.Product.Id, new
        {
            orderResponse.Product.Id,
            Quantity = stockResponse.Quantity - orderResponse.Quantity
        });
    
        if (!response.IsSuccessStatusCode)
            throw new Exception("Sorry, something went wrong while updating the product stock.");
    }
}
