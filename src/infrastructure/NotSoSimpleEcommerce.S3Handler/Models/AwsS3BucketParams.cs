namespace NotSoSimpleEcommerce.S3Handler.Models;

public sealed record AwsS3BucketParams(string BucketName, int UriExpirationMinutes)
{
    public AwsS3BucketParams(): this(string.Empty, int.MinValue){}
}

