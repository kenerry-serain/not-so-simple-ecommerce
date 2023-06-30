using System.Net.Http;
using MediatR;
using SimpleEcommerceV2.Order.Domain.Commands;
using SimpleEcommerceV2.Order.Domain.Events;
using SimpleEcommerceV2.Order.Domain.HttpHandlers.Contracts;
using SimpleEcommerceV2.Order.Domain.InOut.Responses;
using SimpleEcommerceV2.Order.Domain.Mappings;
using SimpleEcommerceV2.Order.Domain.Models;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Order.Domain.CommandHandlers;

public sealed class RegisterOrderCommandHandler : IRequestHandler<RegisterOrderCommand, OrderResponse>
{
    private readonly IMainApi _httpClient;
    private readonly IMediator _mediator;
    private readonly ICreateEntityRepository<OrderEntity> _createRepository;

    public RegisterOrderCommandHandler
    (
        ICreateEntityRepository<OrderEntity> repository, 
        IMainApi httpClient,
        IMediator mediator
    )
    {
        _createRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<OrderResponse> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
    {
        var requestedOrderEntity = request.MapToEntity();
        var stockApiResponse = await _httpClient.GetStockByProductIdAsync(request.ProductId);
        if (!stockApiResponse.IsSuccessStatusCode)
            throw new KeyNotFoundException("Sorry, something went wrong while consulting the product stock.");

        var stockResponse = stockApiResponse.Content;
        if (stockResponse.Quantity < request.Quantity)
            throw new KeyNotFoundException("There is not enought stock for the selected Product.");

        requestedOrderEntity.Quantity = stockResponse.Quantity - request.Quantity;
        var response = await _httpClient.UpdateAsync(request.ProductId, requestedOrderEntity);
        if (!response.IsSuccessStatusCode)
            throw new KeyNotFoundException("Sorry, something went wrong while updating the product stock.");

        var createdOrderEntity = await _createRepository.ExecuteAsync(requestedOrderEntity, cancellationToken);
        await _mediator.Publish(new RegisteredOrderEvent(), cancellationToken);
        return createdOrderEntity.MapToResponse();
    }
}
