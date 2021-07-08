using System;
using static Nano.Net.Utils;

namespace Nano.Net
{
    public class Account
    {
        public byte[] PrivateKey { get; }
        public byte[] PublicKey { get; }
        public string Address { get; }

        public string Frontier { get; set; }
        public Amount Balance { get; set; }
        public string Representative { get; set; }

        public Account(byte[] privateKey)
        {
            PrivateKey = privateKey;
            PublicKey = PublicKeyFromPrivateKey(PrivateKey);
            Address = AddressFromPublicKey(PublicKey);
        }

        public Account(string privateKey)
        {
            PrivateKey = HexToBytes(privateKey);
            PublicKey = PublicKeyFromPrivateKey(PrivateKey);
            Address = AddressFromPublicKey(PublicKey);
        }
    }
}
