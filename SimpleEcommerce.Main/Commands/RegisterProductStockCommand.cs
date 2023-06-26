using MediatR;
using SimpleEcommerce.Main.InOut.Responses;

namespace SimpleEcommerce.Main.Commands
{
    internal sealed class RegisterProductStockCommand : IRequest<StockResponse>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
