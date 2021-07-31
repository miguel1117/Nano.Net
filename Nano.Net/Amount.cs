using System.Numerics;
using Nano.Net.Numbers;

namespace Nano.Net
{
    // not tested enough
    public class Amount
    {
        public BigInteger Raw { get; }
        public BigDecimal Nano => RawToNano(Raw);

        private Amount(BigInteger rawAmount)
        {
            Raw = rawAmount;
        }

        public static Amount FromRaw(string rawAmount)
        {
            return new Amount(BigInteger.Parse(rawAmount));
        }

        public static Amount FromNano(string nanoAmount)
        {
            return new Amount(NanoToRaw(nanoAmount));
        }

        public static BigDecimal RawToNano(BigInteger rawAmount)
        {
            var raw = (BigDecimal) rawAmount;
            var toNano = BigDecimal.Parse("0.000000000000000000000000000001");
            
            return raw * toNano;
        }

        public static BigDecimal RawToNano(string rawAmount)
        {
            return RawToNano(BigInteger.Parse(rawAmount));
        }

        public static BigInteger NanoToRaw(BigDecimal nanoAmount)
        {
            var toRaw = BigDecimal.Parse("1000000000000000000000000000000");
            
            return (BigInteger) (nanoAmount * toRaw);
        }

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
