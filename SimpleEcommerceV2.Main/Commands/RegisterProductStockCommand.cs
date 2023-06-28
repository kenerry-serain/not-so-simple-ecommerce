using MediatR;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Main.Domain.Commands
{
    public sealed class RegisterProductStockCommand : IRequest<StockResponse>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
