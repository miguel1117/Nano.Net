using System.Numerics;
using System.Runtime.Serialization;
using Nano.Net.Numbers;

namespace Nano.Net
{
    // not tested enough
    [DataContract]
    public class Amount
    {
        [DataMember]
        public BigInteger Raw { get; }
        public BigDecimal Nano => RawToNano(Raw);

        private Amount(BigInteger rawAmount)
        {
            Raw = rawAmount;
        }

        /// <summary>
        /// Get amount from raw value
        /// </summary>
        /// <param name="rawAmount">Raw amount as string</param>
        /// <returns></returns>
        public static Amount FromRaw(string rawAmount)
        {
            return new Amount(BigInteger.Parse(rawAmount));
        }

        /// <summary>
        /// Get Amount from Nano value
        /// </summary>
        /// <param name="nanoAmount">Nano amount as string</param>
        /// <returns></returns>
        public static Amount FromNano(string nanoAmount)
        {
            return new Amount(NanoToRaw(nanoAmount));
        }

        /// <summary>
        /// Convert raw to Nano
        /// </summary>
        /// <param name="rawAmount">Raw</param>
        /// <returns></returns>
        public static BigDecimal RawToNano(BigInteger rawAmount)
        {
            var raw = (BigDecimal) rawAmount;
            var toNano = BigDecimal.Parse("0.000000000000000000000000000001");
            
            return raw * toNano;
        }

        /// <summary>
        /// Convert raw to Nano
        /// </summary>
        /// <param name="rawAmount">Raw</param>
        /// <returns></returns>
        public static BigDecimal RawToNano(string rawAmount)
        {
            return RawToNano(BigInteger.Parse(rawAmount));
        }

        /// <summary>
        /// Convert Nano to raw
        /// </summary>
        /// <param name="nanoAmount"></param>
        /// <returns></returns>
        public static BigInteger NanoToRaw(BigDecimal nanoAmount)
        {
            var toRaw = BigDecimal.Parse("1000000000000000000000000000000");
            
            return (BigInteger) (nanoAmount * toRaw);
        }

        /// <summary>
        /// Convert Nano to raw
        /// </summary>
        /// <param name="nanoAmount"></param>
        /// <returns></returns>
        public static BigInteger NanoToRaw(string nanoAmount)
        {
            return NanoToRaw(BigDecimal.Parse(nanoAmount));
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}
