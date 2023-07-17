using System.Net;
using NotSoSimpleEcommerce.S3Handler.Models;

namespace NotSoSimpleEcommerce.S3Handler.Abstractions;

public interface IObjectManager
{
    Task<string> GetPreSignedUrlAsync(string objectKey, CancellationToken cancellationToken);
    Task<string> PutObjectAsync(ObjectRegister @object, CancellationToken cancellationToken);
    Task<HttpStatusCode> DeleteObjectAsync(string objectKey, CancellationToken cancellationToken);
}
