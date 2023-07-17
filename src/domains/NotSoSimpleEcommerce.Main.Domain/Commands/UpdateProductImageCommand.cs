using MediatR;
using Microsoft.AspNetCore.Http;

namespace NotSoSimpleEcommerce.Main.Domain.Commands
{
    public sealed class UpdateProductImageCommand: IRequest<string>
    {
        public UpdateProductImageCommand(int id, IFormFile image)
        {
            Id = id;
            Image = image;
        }

        public int Id { get; }
        public IFormFile Image{ get; init; }
    }
}
