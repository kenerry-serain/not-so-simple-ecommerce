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
        private readonly ICreateEntityRepository<StockEntity> _createEntityRepository;
        private readonly IStockReadRepository _readRepository;


        public UpdateProductStockCommandHandler
        (
            ICreateEntityRepository<StockEntity> createEntityRepository,
            IStockReadRepository readRepository
        )
        {
            _createEntityRepository = createEntityRepository ?? throw new ArgumentNullException(nameof(createEntityRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));

        }

        public async Task<StockResponse> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var oldStock = await _readRepository.GetByProductIdAsync(request.ProductId);
            if (oldStock is null)
                throw new KeyNotFoundException("The specified product doesn't exist.");

            var newStock = request
                .WithId(oldStock.Id)
                .MapToEntity()
                .ReduceQuantity(oldStock.Quantity);
            await _createEntityRepository.ExecuteAsync(newStock, cancellationToken);
            return newStock.MapToResponse();
        }
    }
}
