using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using NotSoSimpleEcommerce.S3Handler.Abstractions;
using NotSoSimpleEcommerce.S3Handler.Models;
using NotSoSimpleEcommerce.Utils.Encryption;

namespace NotSoSimpleEcommerce.S3Handler.Implementations;

public class AwsS3ObjectManager: IObjectManager
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsS3BucketParams _awsS3BucketParams;
    
    public AwsS3ObjectManager
    (
        IAmazonS3 s3Client,
        AwsS3BucketParams awsS3BucketParams
    )
    {
        _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
        _awsS3BucketParams = awsS3BucketParams ?? throw new ArgumentNullException(nameof(awsS3BucketParams));
    }
    
    public Task<string> GetPreSignedUrlAsync(string objectKey, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<string>(cancellationToken);

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _awsS3BucketParams.BucketName,
            Key = objectKey,
            Expires = DateTime.Now.AddMinutes(_awsS3BucketParams.UriExpirationMinutes),
        };

        return Task.FromResult(_s3Client.GetPreSignedURL(request));

    }

    public Task<string> PutObjectAsync(ObjectRegister @object, CancellationToken cancellationToken)
    {
        if (@object is null)
            throw new ArgumentNullException(nameof(@object));

        if (string.IsNullOrWhiteSpace(@object.BucketName) && string.IsNullOrWhiteSpace(_awsS3BucketParams.BucketName))
            throw new ArgumentNullException(nameof(@object.BucketName));

        if (@object.Object == null)
            throw new ArgumentNullException(nameof(@object.Object));

        return PutObjectInternalAsync(@object, cancellationToken);
    }
    
    public Task<HttpStatusCode> DeleteObjectAsync(string objectKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(objectKey))
            throw new ArgumentNullException(nameof(objectKey));

        return DeleteObjectInternalAsync(objectKey, cancellationToken);
    }

    private async Task<HttpStatusCode> DeleteObjectInternalAsync(string objectKey, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            Key = objectKey,
            BucketName = _awsS3BucketParams.BucketName
        };

        var response = await _s3Client.DeleteObjectAsync(request, cancellationToken);
        return response.HttpStatusCode;
    }
    
    private async Task<string> PutObjectInternalAsync(ObjectRegister @params, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = string.IsNullOrWhiteSpace(@params.BucketName) 
                ? _awsS3BucketParams.BucketName 
                : @params.BucketName,
            InputStream = new MemoryStream(@params.Object),
            Key = string.IsNullOrWhiteSpace(@params.ObjectKey) 
                ? Guid.NewGuid().ToString()
                : @params.ObjectKey,
            ContentType = @params.ContentType
        };

        if (@params.Metadata != null)
        {
            foreach (var (key, value) in @params.Metadata)
                request.Metadata.Add(key, value);
        }

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);
        var objectMd5 = Hasher.GetMd5(@params.Object);
        if (objectMd5 != response.ETag?.Trim('"') || string.IsNullOrWhiteSpace(response.ETag))
            throw new ArgumentException("The object is corrupted.");

        var preSignedUrl = await GetPreSignedUrlAsync(request.Key, cancellationToken);
        return preSignedUrl;
    }
}
