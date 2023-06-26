using MediatR;
using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.InOut.Responses;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Mappings;

namespace SimpleEcommerce.Main.CommandHandlers
{
    internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
    {
        private readonly IProductWriteRepository _writeRepository;
        private readonly IProductReadRepository _readRepository;

        public UpdateProductCommandHandler
        (
            IProductWriteRepository writeRepository,
            IProductReadRepository readRepository
        )
        {
            _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(request.Id);
            if (product is null)
                throw new KeyNotFoundException("The specified product doesn't exist.");
            
            var productEntity = await _writeRepository.UpdateAsync(request.MapToEntity());
            return productEntity.MapToResponse();
        }
    }
}
