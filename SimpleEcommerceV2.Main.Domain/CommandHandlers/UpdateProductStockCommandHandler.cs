using MediatR;
using SimpleEcommerceV2.Main.Domain.Commands;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;
using SimpleEcommerceV2.Main.Domain.Mappings;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Main.Domain.Repositories.Contracts;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Domain.CommandHandlers
{
    public sealed class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, StockResponse>
    {
        private readonly IUpdateEntityRepository<StockEntity> _updateEntityRepository;
        private readonly IStockReadRepository _readRepository;

        public UpdateProductStockCommandHandler
        (
            IUpdateEntityRepository<StockEntity> updateEntityRepository,
            IStockReadRepository readRepository
        )
        {
            _updateEntityRepository = updateEntityRepository ?? throw new ArgumentNullException(nameof(updateEntityRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public async Task<StockResponse> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var oldStock = await _readRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
            if (oldStock is null)
                throw new KeyNotFoundException("The specified product doesn't have stock.");

            var newStock = request
                .WithId(oldStock.Id)
                .MapToEntity();

            await _updateEntityRepository.ExecuteAsync(newStock, cancellationToken);
            return newStock.MapToResponse();
        }
    }
}
