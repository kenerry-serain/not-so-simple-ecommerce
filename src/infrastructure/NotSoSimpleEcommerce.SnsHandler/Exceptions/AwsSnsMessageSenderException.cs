using System.Runtime.Serialization;

namespace NotSoSimpleEcommerce.SnsHandler.Exceptions;

[Serializable]
public class AwsSnsMessageSenderException : Exception
{
    public AwsSnsMessageSenderException()
    {
    }

    public AwsSnsMessageSenderException(string message) : base(message)
    {
    }

    public AwsSnsMessageSenderException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected AwsSnsMessageSenderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
