using System;
using System.Globalization;
using System.Numerics;

namespace Nano.Net
{
    public class Amount
    {
        public static readonly BigInteger NanoInRaw = BigInteger.Parse("1000000000000000000000000000000");

        public BigInteger Raw { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Nano => RawToNano(Raw);

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

        public static string RawToNano(BigInteger rawAmount)
        {
            BigInteger result = BigInteger.DivRem(rawAmount, NanoInRaw, out BigInteger remainder);

            return $"{result}.{remainder}"; // only way I know of to not lose precision
        }

        public static string RawToNano(string rawAmount)
        {
            return RawToNano(BigInteger.Parse(rawAmount));
        }

        public static BigInteger NanoToRaw(string nanoAmount)
        {
            if (!decimal.TryParse(nanoAmount, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal _))
                throw new FormatException();

            string[] nano = nanoAmount.Split("."); // split the integer and decimal part

            return BigInteger.Parse(nano[0] + nano[1].PadRight(30, '0'));
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}
