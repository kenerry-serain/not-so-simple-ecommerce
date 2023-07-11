using MediatR;
using SimpleEcommerceV2.Order.Domain.Commands;
using SimpleEcommerceV2.Order.Domain.Events;
using SimpleEcommerceV2.Order.Domain.Mappings;
using SimpleEcommerceV2.Order.Domain.Models;
using SimpleEcommerceV2.Repositories.Contracts;
using SimpleEcommerceV2.Shared.HttpHandlers.Contracts;
using SimpleEcommerceV2.Shared.InOut.Responses;

namespace SimpleEcommerceV2.Order.Domain.CommandHandlers;

public sealed class OrderCreateCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IMainApi _mainApiClient;
    private readonly IMediator _mediator;
    private readonly ICreateEntityRepository<OrderEntity> _createRepository;

    public OrderCreateCommandHandler
    (
        ICreateEntityRepository<OrderEntity> repository, 
        IMainApi mainApiClient,
        IMediator mediator
    )
    {
        _createRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mainApiClient = mainApiClient ?? throw new ArgumentNullException(nameof(mainApiClient));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var requestedOrderEntity = request.MapToEntity();
        var stockApiResponse = await _mainApiClient.GetStockByProductIdAsync(request.ProductId);
        if (!stockApiResponse.IsSuccessStatusCode)
            throw new KeyNotFoundException("Sorry, something went wrong while consulting the product stock.");

        var stockResponse = stockApiResponse.Content;
        if (stockResponse.Quantity < request.Quantity)
            throw new Exception("There is not enough stock for the selected Product.");

        var createdOrderEntity = await _createRepository.ExecuteAsync(requestedOrderEntity, cancellationToken);
        await _mediator.Publish(new OrderCreatedEvent(request), cancellationToken);
        return createdOrderEntity.MapToResponse();
    }
}
