using MediatR;
using SimpleEcommerceV2.Main.Domain.Commands;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;
using SimpleEcommerceV2.Main.Domain.Mappings;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Domain.CommandHandlers
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
