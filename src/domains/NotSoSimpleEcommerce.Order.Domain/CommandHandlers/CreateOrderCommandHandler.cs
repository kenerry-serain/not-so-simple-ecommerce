using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using NotSoSimpleEcommerce.Order.Domain.Commands;
using NotSoSimpleEcommerce.Order.Domain.Events;
using NotSoSimpleEcommerce.Order.Domain.Mappings;
using NotSoSimpleEcommerce.Order.Domain.Repositories.Contracts;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.HttpHandlers.Contracts;
using NotSoSimpleEcommerce.Shared.InOut.Responses;
using NotSoSimpleEcommerce.Shared.Models;
using Microsoft.IdentityModel.Tokens;
namespace NotSoSimpleEcommerce.Order.Domain.CommandHandlers;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IMainApi _mainApiClient;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICreateEntityRepository<OrderEntity> _createRepository;
    private readonly IOrderReadRepository _orderRepository;
    public CreateOrderCommandHandler
    (
        ICreateEntityRepository<OrderEntity> repository, 
        IMainApi mainApiClient,
        IMediator mediator,
        IHttpContextAccessor  httpContextAccessor,
        IOrderReadRepository orderRepository
    )
    {
        _createRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mainApiClient = mainApiClient ?? throw new ArgumentNullException(nameof(mainApiClient));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _orderRepository = orderRepository?? throw new ArgumentNullException(nameof(orderRepository));
        _httpContextAccessor = httpContextAccessor?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(claim => claim.Type.Contains(JwtRegisteredClaimNames.Email))
            ?.Value;
        if (string.IsNullOrWhiteSpace(loggedUser))
            throw new ArgumentNullException();
        
        var requestedOrderEntity = request
            .WithBoughBy(loggedUser)
            .MapToEntity();
        
        var stockApiResponse = await _mainApiClient.GetStockByProductIdAsync(request.ProductId);
        if (!stockApiResponse.IsSuccessStatusCode)
            throw new KeyNotFoundException("Sorry, something went wrong while consulting the product stock.");

        var stockResponse = stockApiResponse.Content;
        if (stockResponse.Quantity < request.Quantity)
            throw new Exception("There is not enough stock for the selected Product.");

        await _createRepository.ExecuteAsync(requestedOrderEntity, cancellationToken);
        await _mediator.Publish(new OrderCreatedEvent(request), cancellationToken);
                
        var createdOrderEntity = await _orderRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        if (createdOrderEntity == null)
            throw new ArgumentNullException();
        
        return createdOrderEntity.MapToResponse();
    }
}
