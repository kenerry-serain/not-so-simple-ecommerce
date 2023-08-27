using MediatR;
using NotSoSimpleEcommerce.Shared.InOut.Responses;

namespace NotSoSimpleEcommerce.Main.Domain.Commands
{
    public sealed class UpdateProductStockCommand : IRequest<StockResponse>
    {
        public int Id { get; private set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public UpdateProductStockCommand WithId(int id)
        {
            Id = id;
            return this;
        }
    }
}
