using MediatR;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Main.Domain.Commands
{
    public sealed class RegisterProductCommand: IRequest<ProductResponse>
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
