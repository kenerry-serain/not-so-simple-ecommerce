using MediatR;
using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Order.Domain.Commands;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Domain.CommandHandlers
{
    public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IDeleteEntityRepository<OrderEntity> _deleteEntityRepository;
        private readonly IReadEntityRepository<OrderEntity> _readRepository;

        public DeleteOrderCommandHandler
        (
            IDeleteEntityRepository<OrderEntity> deleteEntityRepository,
            IReadEntityRepository<OrderEntity> readRepository
        )
        {
            _deleteEntityRepository = deleteEntityRepository ?? throw new ArgumentNullException(nameof(deleteEntityRepository));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));

        }

        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _readRepository.GetAll()
                .FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken: cancellationToken);
            
            if (entity is not null)
                await _deleteEntityRepository.ExecuteAsync(entity, cancellationToken);
        }
    }
}
