using System;

namespace Nano.Net
{
    public class IncompleteBlockException : Exception
    {
        public IncompleteBlockException(string message) : base(message)
        {
        }
    }
}
