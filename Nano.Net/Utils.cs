using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Blake2Sharp;
using Chaos.NaCl;

namespace Nano.Net
{
    public static class Utils
    {
        private static readonly Dictionary<char, string> NanoAddressEncoding;
        private static readonly Dictionary<string, char> NanoAddressDecoding;

        static Utils()
        {
            const string nanoAlphabet = "13456789abcdefghijkmnopqrstuwxyz";
            NanoAddressEncoding = new Dictionary<char, string>();
            NanoAddressDecoding = new Dictionary<string, char>();

            for (var i = 0; i < nanoAlphabet.Length; i++)
            {
                char c = nanoAlphabet[i];

                NanoAddressEncoding[c] = Convert.ToString(i, 2).PadLeft(5, '0');
                NanoAddressDecoding[Convert.ToString(i, 2).PadLeft(5, '0')] = c;
            }
        }

        public static byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Hex string length isn't valid.");

            if (!Regex.IsMatch(hex, @"^(?i)[0-9A-F]+$"))
                throw new ArgumentException("Invalid hex characters.");

            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string BytesToHex(byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString().ToUpper();
        }

        public static string GenerateSeed()
        {
            var seed = new byte[32];

            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(seed);

            return BytesToHex(seed);
        }

        private static string EncodeNanoBase32(byte[] data, bool padZeros = true)
        {
            string binaryString = padZeros ? "0000" : string.Empty;

            foreach (byte t in data)
                binaryString += Convert.ToString(t, 2).PadLeft(8, '0');

            var result = string.Empty;

            for (var i = 0; i < binaryString.Length; i += 5)
                result += NanoAddressDecoding[binaryString.Substring(i, 5)];

            return result;
        }

        private static byte[] DecodeNanoBase32(string data)
        {
            int prefixIndex = data.IndexOf("_", StringComparison.Ordinal);

            if (prefixIndex == -1)
                throw new ArgumentException("This Nano address isn't valid.");

            data = data.Substring(prefixIndex + 1, 52);

            var binaryString = string.Empty;
            foreach (char t in data)
                binaryString += NanoAddressEncoding[t]; // Decode each character into string representation of it's binary parts

            binaryString = binaryString[4..]; // Remove leading 4 0s

            // Convert to bytes
            var publicKeyBytes = new byte[32];
            for (var i = 0; i < 32; i++)
            {
                var b = Convert.ToByte(binaryString.Substring(i * 8, 8), 2); // for each byte, read the bits from the binary string
                publicKeyBytes[i] = b;
            }

            return publicKeyBytes;
        }

        internal static byte[] Blake2BHash(int sizeInBytes, params byte[][] data)
        {
            Hasher hasher = Blake2B.Create(new Blake2BConfig() { OutputSizeInBytes = sizeInBytes });
            hasher.Init();

            foreach (byte[] bytes in data)
                hasher.Update(bytes);

            return hasher.Finish();
        }

        public static byte[] DerivePrivateKey(string seed, uint index)
        {
            if (seed.Length != 64)
                throw new ArgumentException("A Nano seed should consist of 64 hex characters.");

            byte[] indexBytes = BitConverter.GetBytes(index);
            if (BitConverter.IsLittleEndian)
                indexBytes = indexBytes.Reverse().ToArray();

            byte[] seedBytes = HexToBytes(seed);

            return Blake2BHash(32, seedBytes, indexBytes);
        }

        public static byte[] PublicKeyFromPrivateKey(byte[] privateKey)
        {
            if (privateKey.Length != 32)
                throw new ArgumentException("A private key must be exactly 32 bytes.");

            Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] _, privateKey);
            return publicKey;
        }

        private static string EncodedAddressChecksum(byte[] publicKey)
        {
            byte[] final = Blake2BHash(5, publicKey);

            return EncodeNanoBase32(final.Reverse().ToArray(), false);
        }

        public static string AddressFromPublicKey(byte[] publicKey)
        {
            if (publicKey.Length != 32)
                throw new ArgumentException("A public key must be exactly 32 bytes.");

            var address = "nano_";
            address += EncodeNanoBase32(publicKey);
            address += EncodedAddressChecksum(publicKey);

            return address;
        }

        public static byte[] PublicKeyFromAddress(string address)
        {
            if (IsAddressValid(address))
                return DecodeNanoBase32(address);
            else
                throw new ArgumentException("This Nano address isn't valid.");
        }

        public static bool IsAddressValid(string address)
        {
            if (!Regex.IsMatch(address, @"^(nano|xrb)_[13]{1}[13456789abcdefghijkmnopqrstuwxyz]{59}$"))
                return false;

            byte[] publicKey = DecodeNanoBase32(address);

            return EncodedAddressChecksum(publicKey) == address[^8..];
        }
    }
}
