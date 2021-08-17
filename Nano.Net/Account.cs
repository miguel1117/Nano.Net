using System;
using static Nano.Net.Utils;

namespace Nano.Net
{
    public class Account
    {
        public byte[] PrivateKey { get; private init; }
        public byte[] PublicKey { get; private init; }
        public string Address { get; private init; }

        public bool Opened { get; set; } = true;
        public string Frontier { get; set; }
        public Amount Balance { get; set; }
        public string Representative { get; set; }

        /// <summary>Whether all the properties for this account have been set.</summary>
        internal bool MissingInformation => Frontier is null || Balance is null || Representative is null;

        private Account()
        {
        }

        /// <summary>
        /// Create account from seed
        /// </summary>
        /// <param name="seed">The seed used to derivate the private key for this account</param>
        /// <param name="index">The index used to derive the key</param>
        /// <returns></returns>
        public static Account FromSeed(string seed, uint index)
        {
            return FromPrivateKey(DerivePrivateKey(seed, index));
        }

        /// <summary>
        /// Create account from private key
        /// </summary>
        public static Account FromPrivateKey(byte[] privateKey)
        {
            byte[] publicKey = PublicKeyFromPrivateKey(privateKey);
            string address = AddressFromPublicKey(publicKey);
            
            return new Account()
            {
                PrivateKey = privateKey,
                PublicKey = publicKey,
                Address = address
            };
        }

        /// <summary>
        /// Create account from private key
        /// </summary>
        public static Account FromPrivateKey(string privateKey)
        {
            return FromPrivateKey(HexToBytes(privateKey));
        }
    }
}
