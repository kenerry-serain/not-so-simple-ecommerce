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
    private readonly ISimpleHttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly ICreateEntityRepository<OrderEntity> _createRepository;

    public RegisterOrderCommandHandler
    (
        ICreateEntityRepository<OrderEntity> repository, 
        ISimpleHttpClient httpClient,
        IMediator mediator
    )
    {
        _createRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<OrderResponse> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = request.MapToEntity();
        var response = await _httpClient.UpdateAsync(request.ProductId, orderEntity);
        if (!response.IsSuccessStatusCode)
            throw new ArgumentException($"Your order was not completed, check if the product exists and if the " +
                                        "stock is enough.");

        orderEntity = await _createRepository.ExecuteAsync(orderEntity, cancellationToken);
        
        await _mediator.Publish(new RegisteredOrderEvent(), cancellationToken);
        return orderEntity.MapToResponse();
    }
}
