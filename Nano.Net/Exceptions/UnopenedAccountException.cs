using System;

namespace Nano.Net
{
    public class UnopenedAccountException : Exception
    {
        public UnopenedAccountException() : base("This account hasn't been opened yet.")
        {
        }
    }
}
