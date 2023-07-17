using MediatR;

namespace NotSoSimpleEcommerce.Main.Domain.Commands;

public sealed class DeleteProductImageCommand: IRequest<bool>
{
    public DeleteProductImageCommand(int id, string objectKey)
    {
        Id = id;
        ObjectKey = objectKey;
    }

    public int Id { get; set; }
    public string ObjectKey { get; set; }
}
