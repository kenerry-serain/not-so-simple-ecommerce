using MediatR;
using SimpleEcommerce.Main.InOut.Responses;

namespace SimpleEcommerce.Main.Commands
{
    internal sealed class UpdateProductStockCommand : IRequest<StockResponse>
    {
        public int Id { get; private set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        internal UpdateProductStockCommand WithId(int id)
        {
            Id = id;
            return this;
        }
    }
}
