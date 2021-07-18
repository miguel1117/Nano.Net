using System.Numerics;
using Xunit;

namespace Nano.Net.Tests
{
    public class AmountTest
    {
        [Fact]
        public void NanoToRawTest()
        {
            Assert.Equal(BigInteger.Parse("10516572890244336648297290184986670415"), Amount.NanoToRaw("10516572.890244336648297290184986670415"));
        }

        [Fact]
        public void RawToNanoTest()
        {
            Assert.Equal("10516572.890244336648297290184986670415", Amount.RawToNano("10516572890244336648297290184986670415"));
        }
    }
}
