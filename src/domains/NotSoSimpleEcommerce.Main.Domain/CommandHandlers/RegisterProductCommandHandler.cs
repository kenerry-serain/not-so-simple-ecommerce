using MediatR;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Main.Domain.InOut.Responses;
using NotSoSimpleEcommerce.Main.Domain.Mappings;
using NotSoSimpleEcommerce.Main.Domain.Models;
using NotSoSimpleEcommerce.Repositories.Contracts;

namespace NotSoSimpleEcommerce.Main.Domain.CommandHandlers
{
    public sealed class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, ProductResponse>
    {
        private readonly ICreateEntityRepository<ProductEntity> _createEntityRepository;

        public RegisterProductCommandHandler
        (
            ICreateEntityRepository<ProductEntity> createEntityRepository
        )
        {
            _createEntityRepository = createEntityRepository ?? throw new ArgumentNullException(nameof(createEntityRepository));
        }

        public async Task<ProductResponse> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = await _createEntityRepository.ExecuteAsync(request.MapToEntity(), cancellationToken);
            return productEntity.MapToResponse();
        }
    }
}
