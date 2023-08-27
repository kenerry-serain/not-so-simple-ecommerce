using MediatR;
using NotSoSimpleEcommerce.Shared.InOut.Responses;

namespace NotSoSimpleEcommerce.Main.Domain.Commands
{
    public sealed class RegisterProductCommand: IRequest<ProductResponse>
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
