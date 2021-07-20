using System.Numerics;

namespace Nano.Net
{
    public class Amount
    {
        public BigInteger Raw { get; }

        private Amount(BigInteger rawAmount)
        {
            Raw = rawAmount;
        }

        public static Amount FromRaw(string rawAmount)
        {
            return new Amount(BigInteger.Parse(rawAmount));
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}
