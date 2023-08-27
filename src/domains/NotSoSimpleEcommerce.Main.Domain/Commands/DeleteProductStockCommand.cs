using MediatR;

namespace NotSoSimpleEcommerce.Main.Domain.Commands;

public class DeleteProductStockCommand: IRequest
{
    public DeleteProductStockCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}
