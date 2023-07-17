using MediatR;
using Microsoft.Extensions.Configuration;
using NotSoSimpleEcommerce.Main.Domain.Commands;
using NotSoSimpleEcommerce.S3Handler.Abstractions;
using NotSoSimpleEcommerce.S3Handler.Models;

namespace NotSoSimpleEcommerce.Main.Domain.CommandHandlers;

public sealed class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand, string>
{
    private readonly IObjectManager _objectManager;
    private readonly IConfiguration _configuration;

    public UpdateProductImageCommandHandler
    (
        IObjectManager objectManager,
        IConfiguration configuration
    )
    {
        _objectManager = objectManager ?? throw new ArgumentNullException(nameof(objectManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<string> Handle(UpdateProductImageCommand command, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();
        await command.Image.CopyToAsync(stream, cancellationToken);
        var imageBytes = stream.ToArray();
        if (imageBytes.Length == 0)
            throw new ArgumentNullException(nameof(command.Image));
        
        //TODO add content-type validation
        var objectRegister = new ObjectRegister
        (
            _configuration.GetValue<string>("BucketName")!,
            $"ProductId-{command.Id}-{Guid.NewGuid().ToString()}",
            imageBytes,
            command.Image.ContentType
        );

        var objectUrl = await _objectManager.PutObjectAsync(objectRegister, cancellationToken);
        return objectUrl;
    }
}

