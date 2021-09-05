using System;

namespace Nano.Net
{
    public class NanoWebSocketException : Exception
    {
        public NanoWebSocketException(string message) : base(message)
        {
        }
    }
}
