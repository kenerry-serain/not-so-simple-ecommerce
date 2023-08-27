using MediatR;
using NotSoSimpleEcommerce.Shared.InOut.Responses;

namespace NotSoSimpleEcommerce.Main.Domain.Commands
{
    public sealed class RegisterProductStockCommand : IRequest<StockResponse>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
