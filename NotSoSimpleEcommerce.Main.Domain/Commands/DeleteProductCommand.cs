using MediatR;

namespace NotSoSimpleEcommerce.Main.Domain.Commands
{
    public sealed class DeleteProductCommand: IRequest
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
