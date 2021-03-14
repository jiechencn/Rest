using System;

namespace Xzoo.Rest.Client
{
    public class RestException : Exception
    {
        public RestException(string message) : base(message)
        {
        }

        public RestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}