using MediatR;
using SimpleEcommerceV2.Main.Domain.InOut.Responses;

namespace SimpleEcommerceV2.Main.Domain.Commands
{
    public sealed class UpdateProductCommand: IRequest<ProductResponse>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
