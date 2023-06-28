using MediatR;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Main.Domain.Commands
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
