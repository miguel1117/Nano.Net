using System;

namespace Nano.Net
{
    public class RpcException : Exception
    {
        public RpcException(string message) : base(message)
        {
        }
    }
}
