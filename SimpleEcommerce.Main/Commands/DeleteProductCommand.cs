using MediatR;

namespace SimpleEcommerce.Main.Commands
{
    internal sealed class DeleteProductCommand: IRequest
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
