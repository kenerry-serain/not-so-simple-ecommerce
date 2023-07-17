namespace NotSoSimpleEcommerce.S3Handler.Models;

public record ObjectRegister
(
    string BucketName, 
    string ObjectKey, 
    byte[] Object, 
    string ContentType= "application/json",
    IDictionary<string, string>? Metadata = null
);
