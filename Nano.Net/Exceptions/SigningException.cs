using System;

namespace Nano.Net.Exceptions;

public class SigningException : Exception
{
    public SigningException(string message) : base(message)
    {
    }
}
