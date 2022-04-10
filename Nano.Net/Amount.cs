using System;
using System.Numerics;
using System.Runtime.Serialization;
using Nano.Net.Numbers;

namespace Nano.Net
{
    [DataContract]
    public class Amount : IEquatable<Amount>
    {
        [DataMember]
        public BigInteger Raw { get; }
        public BigDecimal Nano => RawToNano(Raw);

        public Amount(BigInteger rawAmount)
        {
            Raw = rawAmount;
        }

        public static Amount operator +(Amount a, Amount b)
        {
            return new Amount(a.Raw + b.Raw);
        }
        
        public static Amount operator -(Amount a, Amount b)
        {
            return new Amount(a.Raw - b.Raw);
        }

        public static bool operator ==(Amount a, Amount b)
        {
            if (a is null && b is null)
                return true;
            else if (a is null)
                return false;
            else
                return a.Equals(b);
        }
        
        public static bool operator !=(Amount a, Amount b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Creates a new Amount object from an amount in raw.
        /// </summary>
        public static Amount FromRaw(string rawAmount)
        {
            return new Amount(BigInteger.Parse(rawAmount));
        }

        /// <summary>
        /// Creates a new Amount object from an amount in nano.
        /// </summary>
        /// <param name="nanoAmount">Nano amount as string</param>
        public static Amount FromNano(string nanoAmount)
        {
            return new Amount(NanoToRaw(nanoAmount));
        }

        /// <summary>
        /// Converts raw to nano.
        /// </summary>
        public static BigDecimal RawToNano(BigInteger rawAmount)
        {
            var raw = (BigDecimal) rawAmount;
            var toNano = BigDecimal.Parse("0.000000000000000000000000000001");
            
            return raw * toNano;
        }

        /// <summary>
        /// Converts raw to nano.
        /// </summary>
        public static BigDecimal RawToNano(string rawAmount)
        {
            return RawToNano(BigInteger.Parse(rawAmount));
        }

        /// <summary>
        /// Converts nano to raw.
        /// </summary>
        public static BigInteger NanoToRaw(BigDecimal nanoAmount)
        {
            var toRaw = BigDecimal.Parse("1000000000000000000000000000000");
            
            return (BigInteger) (nanoAmount * toRaw);
        }

        /// <summary>
        /// Converts nano to raw.
        /// </summary>
        public static BigInteger NanoToRaw(string nanoAmount)
        {
            return NanoToRaw(BigDecimal.Parse(nanoAmount));
        }

        public override string ToString()
        {
            return Raw.ToString();
        }

        public bool Equals(Amount other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            
            return Raw == other.Raw;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != GetType())
                return false;
            
            return Equals((Amount) obj);
        }

        public override int GetHashCode()
        {
            return Raw.GetHashCode();
        }
    }
}
