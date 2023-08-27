using MediatR;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Main.Domain.Mappings;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.InOut.Responses;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.CommandHandlers
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
