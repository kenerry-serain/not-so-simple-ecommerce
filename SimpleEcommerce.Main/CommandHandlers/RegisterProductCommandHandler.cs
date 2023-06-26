using MediatR;
using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.InOut.Responses;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Mappings;

namespace SimpleEcommerce.Main.CommandHandlers
{
    internal sealed class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, ProductResponse>
    {
        private readonly IProductWriteRepository _repository;
        public RegisterProductCommandHandler(IProductWriteRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProductResponse> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = await _repository.AddAsync(request.MapToEntity());
            return productEntity.MapToResponse();
        }
    }
}
