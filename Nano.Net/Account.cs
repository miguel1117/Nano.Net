using System;
using System.Security.Cryptography;
using Nano.Net.Extensions;
using static Nano.Net.Utils;

namespace Nano.Net
{
    public class Account
    {
        public byte[] PrivateKey { get; }
        public byte[] PublicKey { get; }
        public string Address { get; }

        public bool Opened { get; set; } = true;
        public string Frontier { get; set; }
        public Amount Balance { get; set; }
        public string Representative { get; set; }

        /// <summary>Whether all the properties for this account have been set.</summary>
        internal bool MissingInformation => Frontier is null || Balance is null || Representative is null;

        /// <summary>
        /// Create a new Account object with a random private key and the prefix nano.
        /// </summary>
        public Account() : this("nano") {}
        
        /// <summary>
        /// Create a new Account object with a random private key.
        /// </summary>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        public Account(string prefix = "nano")
        {
            using var rng = new RNGCryptoServiceProvider();

            PrivateKey = new byte[32];
            rng.GetBytes(PrivateKey);
            PublicKey = PublicKeyFromPrivateKey(PrivateKey);
            Address = AddressFromPublicKey(PublicKey, prefix);
        }
        
        /// <summary>
        /// Create an Account object from a private key.
        /// </summary>
        /// <param name="privateKey">Private key bytes bytes.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        public Account(byte[] privateKey, string prefix = "nano")
        {
            PrivateKey = privateKey;
            PublicKey = PublicKeyFromPrivateKey(privateKey);
            Address = AddressFromPublicKey(PublicKey, prefix);
        }
        
        /// <summary>
        /// Create an Account object from a private key.
        /// </summary>
        /// <param name="privateKey">Private key bytes encoded as a hex string.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        public Account(string privateKey, string prefix = "nano") : this(privateKey.HexToBytes(), prefix)
        {
        }
        
        /// <summary>
        /// Create an Account object from a seed.
        /// </summary>
        /// <param name="seed">The seed used to derive the private key for this account.</param>
        /// <param name="index">The index used to derive the key.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        public Account(byte[] seed, uint index, string prefix = "nano") 
        {
            PrivateKey = DerivePrivateKey(seed, index);
            PublicKey = PublicKeyFromPrivateKey(PrivateKey);
            Address = AddressFromPublicKey(PublicKey, prefix);
        }
        
        /// <summary>
        /// Create an Account object from a seed.
        /// </summary>
        /// <param name="seed">The seed as a hex string used to derive the private key for this account.</param>
        /// <param name="index">The index used to derive the key.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        public Account(string seed, uint index, string prefix = "nano") : this(seed.HexToBytes(), index, prefix)
        {
        }

        /// <summary>
        /// Create an Account object from a seed.
        /// </summary>
        /// <param name="seed">The seed as a hex string used to derive the private key for this account.</param>
        /// <param name="index">The index used to derive the key.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        [Obsolete]
        public static Account FromSeed(string seed, uint index, string prefix = "nano")
        {
            return new Account(seed, index, prefix);
        }

        /// <summary>
        /// Create an Account object from a private key.
        /// </summary>
        /// <param name="privateKey">Private key bytes bytes.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        [Obsolete]
        public static Account FromPrivateKey(byte[] privateKey, string prefix = "nano")
        {
            return new Account(privateKey, prefix);
        }

        /// <summary>
        /// Create an Account object from a private key.
        /// </summary>
        /// <param name="privateKey">Private key bytes encoded as a hex string.</param>
        /// <param name="prefix">The prefix used for the address (without the "_"). Defaults to nano.</param>
        [Obsolete]
        public static Account FromPrivateKey(string privateKey, string prefix = "nano")
        {
            return new Account(privateKey, prefix);
        }
    }
}
