using System;
using static Nano.Net.Utils;

namespace Nano.Net
{
    public class Account
    {
        public byte[] PrivateKey { get; private init; }
        public byte[] PublicKey { get; private init; }
        public string Address { get; private init; }

        public string Frontier { get; set; }
        public Amount Balance { get; set; }
        public string Representative { get; set; }
        
        /// <summary>Whether all the properties for this account have been set.</summary>
        internal bool MissingInformation => Frontier is null || Balance is null || Representative is null;

        private Account()
        {
        }

        public static Account FromSeed(string seed, uint index)
        {
            return FromPrivateKey(DerivePrivateKey(seed, index));
        }

        public static Account FromPrivateKey(byte[] privateKey)
        {
            return new Account()
            {
                PrivateKey = privateKey,
                PublicKey = PublicKeyFromPrivateKey(privateKey),
                Address = AddressFromPublicKey(privateKey),
            };
        }

        public static Account FromPrivateKey(string privateKey)
        {
            return FromPrivateKey(HexToBytes(privateKey));
        }
    }
}
