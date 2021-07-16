using System;

namespace Nano.Net
{
    public class InvalidSeedException : Exception
    {
        public InvalidSeedException(string message) : base(message)
        {
        }
    }
}
