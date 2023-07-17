using MediatR;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.S3Handler.Abstractions;

namespace NotSoSimpleEcommerce.Main.Domain.CommandHandlers;

public class DeleteProductImageCommandHandler: IRequestHandler<DeleteProductImageCommand, bool>
{
    private readonly IObjectManager _objectManager;

    public DeleteProductImageCommandHandler(IObjectManager objectManager)
    {
        _objectManager = objectManager ?? throw new ArgumentNullException(nameof(objectManager));
    }
    
    public async Task<bool> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var httpStatusCode = await _objectManager.DeleteObjectAsync(request.ObjectKey, cancellationToken);
        return (int)httpStatusCode is >= 200 and <= 299;
    }
}
