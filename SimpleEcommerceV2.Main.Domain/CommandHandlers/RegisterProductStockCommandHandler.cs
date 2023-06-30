using MediatR;
using SimpleEcommerceV2.Main.Domain.Commands;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;
using SimpleEcommerceV2.Main.Domain.Mappings;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Main.Domain.Repositories.Contracts;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Domain.CommandHandlers
{
    public sealed class RegisterProductStockCommandHandler : IRequestHandler<RegisterProductStockCommand, StockResponse>
    {
        private readonly ICreateEntityRepository<StockEntity> _createEntityRepository;
        private readonly IStockReadRepository _stockReadRepository;

        public RegisterProductStockCommandHandler
        (
            ICreateEntityRepository<StockEntity> createEntityRepository,
            IStockReadRepository stockReadRepository
        )
        {
            _createEntityRepository = createEntityRepository ?? throw new ArgumentNullException(nameof(createEntityRepository));
            _stockReadRepository = stockReadRepository ?? throw new ArgumentNullException(nameof(stockReadRepository));
        }

        public async Task<StockResponse> Handle(RegisterProductStockCommand request, CancellationToken cancellationToken)
        {
            var oldStock = await _stockReadRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
            if (oldStock is not null)
                throw new ArgumentException("The specified product already has a stock registered, please use" +
                                               "the UPDATE method.");
            
            var stockEntity = request.MapToEntity();
            await _createEntityRepository.ExecuteAsync(stockEntity, cancellationToken);
            return stockEntity.MapToResponse();
        }
    }
}
