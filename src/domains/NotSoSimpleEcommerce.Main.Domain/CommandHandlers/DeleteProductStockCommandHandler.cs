using MediatR;
using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Main.Domain.CommandHandlers;

public sealed class DeleteProductStockCommandHandler: IRequestHandler<DeleteProductStockCommand>
{
    private readonly IDeleteEntityRepository<StockEntity> _deleteEntityRepository;
    private readonly IReadEntityRepository<StockEntity> _productReadRepository;

    public DeleteProductStockCommandHandler
    (
        IDeleteEntityRepository<StockEntity> deleteEntityRepository,
        IReadEntityRepository<StockEntity> productReadRepository
    )
    {
        _deleteEntityRepository = deleteEntityRepository ?? throw new ArgumentNullException(nameof(deleteEntityRepository));
        _productReadRepository = productReadRepository ?? throw new ArgumentNullException(nameof(productReadRepository));
    }

    public async Task Handle(DeleteProductStockCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productReadRepository.GetAll()
            .Include(stock => stock.Product)
            .FirstOrDefaultAsync(product => product.Id == request.Id, cancellationToken);
        
        if (entity is not null)
            await _deleteEntityRepository.ExecuteAsync(entity, cancellationToken);
    }
}
