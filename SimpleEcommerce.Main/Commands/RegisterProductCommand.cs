using MediatR;
using SimpleEcommerce.Main.InOut.Responses;

namespace SimpleEcommerce.Main.Commands
{
    internal sealed class RegisterProductCommand: IRequest<ProductResponse>
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
