using System;

namespace Nano.Net
{
    public class AccountMissingInformationException : Exception
    {
        public AccountMissingInformationException() : base(
            "Not all properties for this account have been set. Please update this account's properties manually or use the RpcClient UpdateAccountAsync method.")
        {
        }
    }
}
