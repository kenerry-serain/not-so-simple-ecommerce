using MediatR;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Main.Domain.Mappings;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Contracts;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.InOut.Responses;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.CommandHandlers
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

            var stockToUpdate = request
                .WithId(oldStock.Id)
                .MapToEntity();

            await _updateEntityRepository.ExecuteAsync(stockToUpdate, cancellationToken);
            
            var updatedStock = await _readRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
            if (updatedStock == null)
                throw new ArgumentNullException();
            
            return updatedStock.MapToResponse();
        }
    }
}
