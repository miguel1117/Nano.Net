using System.Globalization;

namespace Nano.Net
{
    public class Amount
    {
        public const double NanoInRaw = 1000000000000000000000000000000D;

        public double Raw { get; } // TODO BigInteger is probably a better choice than double here
        public double Nano => RawToNano(Raw);

        public Amount(double rawAmount)
        {
            Raw = rawAmount;
        }

        public static Amount FromRaw(string rawAmount)
        {
            return new Amount(double.Parse(rawAmount, CultureInfo.InvariantCulture));
        }

        public static Amount FromNano(string nanoAmount)
        {
            return new Amount(NanoToRaw(nanoAmount));
        }

        public static double RawToNano(double rawAmount)
        {
            return rawAmount / NanoInRaw;
        }

        public static double RawToNano(string rawAmount)
        {
            double raw = double.Parse(rawAmount, CultureInfo.InvariantCulture);
            return RawToNano(raw);
        }

        public static double NanoToRaw(double nanoAmount)
        {
            return nanoAmount * NanoInRaw;
        }

        public static double NanoToRaw(string nanoAmount)
        {
            double nano = double.Parse(nanoAmount, CultureInfo.InvariantCulture);
            return NanoToRaw(nano);
        }

        public override string ToString()
        {
            return Nano.ToString(CultureInfo.InvariantCulture);
        }
    }
}
