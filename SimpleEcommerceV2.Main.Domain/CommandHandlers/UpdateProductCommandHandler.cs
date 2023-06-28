using MediatR;
using SimpleEcommerceV2.Main.Domain.Commands;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;
using SimpleEcommerceV2.Main.Domain.Mappings;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Domain.CommandHandlers
{
    internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
    {
        private readonly IUpdateEntityRepository<ProductEntity> _updateEntityRepository;
        private readonly IReadEntityRepository<ProductEntity> _readRepository;

        public UpdateProductCommandHandler
        (
            IUpdateEntityRepository<ProductEntity> updateEntityRepository,
            IReadEntityRepository<ProductEntity> readRepository

        )
        {
            _updateEntityRepository = updateEntityRepository ?? throw new ArgumentNullException(nameof(updateEntityRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product is null)
                throw new KeyNotFoundException("The specified product doesn't exist.");
            
            var productEntity = await _updateEntityRepository.ExecuteAsync(request.MapToEntity(), cancellationToken);
            return productEntity.MapToResponse();
        }
    }
}
