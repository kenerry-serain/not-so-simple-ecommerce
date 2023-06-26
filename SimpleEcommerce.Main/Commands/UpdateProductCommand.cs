using MediatR;
using SimpleEcommerce.Main.InOut.Responses;

namespace SimpleEcommerce.Main.Commands
{
    internal sealed class UpdateProductCommand: IRequest<ProductResponse>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
