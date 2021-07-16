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
        private const string NanoAlphabet = "13456789abcdefghijkmnopqrstuwxyz";

        static Utils()
        {
            NanoAddressEncoding = new Dictionary<char, string>();
            NanoAddressDecoding = new Dictionary<string, char>();

            for (var i = 0; i < NanoAlphabet.Length; i++)
            {
                char c = NanoAlphabet[i];

                NanoAddressEncoding[c] = Convert.ToString(i, 2).PadLeft(5, '0');
                NanoAddressDecoding[Convert.ToString(i, 2).PadLeft(5, '0')] = c;
            }
        }

        public static byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new InvalidHexStringException("Hex string length isn't valid.");
            
            if (!Regex.IsMatch(hex, @"^[0-9A-F]+$"))
                throw new InvalidHexStringException("Invalid hex characters.");

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
                throw new FormatException("Invalid Nano address");

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

        internal static byte[] Blake2BHash(int sizeInBytes, byte[] data)
        {
            Hasher hasher = Blake2B.Create(new Blake2BConfig() { OutputSizeInBytes = sizeInBytes });

            hasher.Init();
            hasher.Update(data);
            return hasher.Finish();
        }

        public static byte[] DerivePrivateKey(string seed, uint index)
        {
            if (seed.Length != 64)
                throw new InvalidSeedException("A Nano seed should consist of 64 hex characters.");

            byte[] indexBytes = BitConverter.GetBytes(index);
            if (BitConverter.IsLittleEndian)
                indexBytes = indexBytes.Reverse().ToArray();

            byte[] seedBytes = HexToBytes(seed);

            return Blake2BHash(32, seedBytes.Concat(indexBytes).ToArray());
        }

        public static byte[] PublicKeyFromPrivateKey(byte[] privateKey)
        {
            Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] _, privateKey);
            return publicKey;
        }

        private static byte[] AddressChecksum(byte[] publicKey)
        {
            byte[] final = Blake2BHash(5, publicKey);

            return final.Reverse().ToArray();
        }

        public static string AddressFromPublicKey(byte[] publicKey)
        {
            var address = "nano_";
            address += EncodeNanoBase32(publicKey);
            address += EncodeNanoBase32(AddressChecksum(publicKey), false);

            return address;
        }

        public static byte[] PublicKeyFromAddress(string address)
        {
            return DecodeNanoBase32(address);
        }
    }
}
