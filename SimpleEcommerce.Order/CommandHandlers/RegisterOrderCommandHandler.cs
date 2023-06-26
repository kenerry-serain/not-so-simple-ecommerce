using MediatR;
using SimpleEcommerce.Order.Commands;
using SimpleEcommerce.Order.HttpHandlers.Contracts;
using SimpleEcommerce.Order.InOut.Responses;
using SimpleEcommerce.Order.Mappings;
using SimpleEcommerce.Order.Repositories.Contracts;

namespace SimpleEcommerce.Order.CommandHandlers;

internal sealed class RegisterOrderCommandHandler : IRequestHandler<RegisterOrderCommand, OrderResponse>
{
    private readonly IOrderWriteRepository _repository;
    private readonly ISimpleHttpClient _httpClient;

    public RegisterOrderCommandHandler(IOrderWriteRepository repository, ISimpleHttpClient httpClient)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<OrderResponse> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = request.MapToEntity();
        var response = await _httpClient.UpdateAsync(request.ProductId, orderEntity);
        if (!response.IsSuccessStatusCode)
            throw new ArgumentException($"Your order was not completed, check if the product exists and if the " +
                                        "stock is enough.");

        orderEntity = await _repository.AddAsync(orderEntity);
        return orderEntity.MapToResponse();
    }
}
