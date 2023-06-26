using MediatR;
using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.Repositories.Contracts;

namespace SimpleEcommerce.Main.CommandHandlers
{
    internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductWriteRepository _repository;
        public DeleteProductCommandHandler(IProductWriteRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
        }
    }
}
