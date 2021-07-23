using System;

namespace Nano.Net
{
    public class RpcException : Exception
    {
        public string OriginalError { get; }

        public RpcException(string errorMessage) : base($"RPC call returned error. Message: {errorMessage}")
        {
            OriginalError = errorMessage;
        }
    }
}
