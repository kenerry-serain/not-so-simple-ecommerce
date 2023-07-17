using System.Runtime.Serialization;

namespace NotSoSimpleEcommerce.SqsHandler.Exceptions
{
    [Serializable]
    public class AwsSqsMessageSenderException : Exception
    {
        public AwsSqsMessageSenderException()
        {
        }

        public AwsSqsMessageSenderException(string message) : base(message)
        {
        }

        public AwsSqsMessageSenderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AwsSqsMessageSenderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
