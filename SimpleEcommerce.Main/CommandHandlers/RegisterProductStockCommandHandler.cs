using MediatR;
using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.InOut.Responses;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Mappings;

namespace SimpleEcommerce.Main.CommandHandlers
{
    internal sealed class RegisterProductStockCommandHandler : IRequestHandler<RegisterProductStockCommand, StockResponse>
    {
        private readonly IStockWriteRepository _writeRepository;
        private readonly IStockReadRepository _readRepository;
        private readonly IProductReadRepository _productReadRepository;

        public RegisterProductStockCommandHandler
        (
            IStockWriteRepository writeRepository,
            IStockReadRepository readRepository,
            IProductReadRepository productReadRepository
        )
        {
            _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
            _productReadRepository = productReadRepository?? throw new ArgumentNullException(nameof(productReadRepository));
        }
        
        public async Task<StockResponse> Handle(RegisterProductStockCommand request, CancellationToken cancellationToken)
        {
            var oldStock = await _readRepository.GetByProductIdAsync(request.ProductId);
            if (oldStock is not null)
                throw new ArgumentException("The specified product already has a stock registered, please use" +
                                               "the UPDATE method.");
            
            var stockEntity = request.MapToEntity();
            await _writeRepository.AddAsync(stockEntity);
            return stockEntity.MapToResponse();
        }
    }
}
