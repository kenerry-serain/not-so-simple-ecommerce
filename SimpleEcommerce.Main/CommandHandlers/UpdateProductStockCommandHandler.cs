using MediatR;
using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.InOut.Responses;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Mappings;

namespace SimpleEcommerce.Main.CommandHandlers
{
    internal sealed class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, StockResponse>
    {
        private readonly IStockWriteRepository _writeRepository;
        private readonly IStockReadRepository _readRepository;

        public UpdateProductStockCommandHandler
        (
            IStockWriteRepository writeRepository,
            IStockReadRepository readRepository
        )
        {
            _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public async Task<StockResponse> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var oldStock = await _readRepository.GetByProductIdAsync(request.ProductId);
            if (oldStock is null)
                throw new KeyNotFoundException("The specified product doesn't exist.");
            
            if (oldStock.Quantity < request.Quantity)
                throw new KeyNotFoundException("There is not enough stock for the order.");

            var newStock = request
                .WithId(oldStock.Id)
                .MapToEntity()
                .ReduceQuantity(oldStock.Quantity);
            await _writeRepository.UpdateAsync(newStock);
            return newStock.MapToResponse();
        }
    }
}
