using System;

namespace Nano.Net
{
    public class InvalidHexStringException : Exception
    {
        public InvalidHexStringException(string message) : base(message)
        {
        }
    }
}
