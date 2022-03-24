using System.Numerics;
using Nano.Net.Numbers;
using Xunit;

namespace Nano.Net.Tests;

public class AmountTest
{
    [Fact]
    public void NanoToRawTest()
    {
        Assert.Equal(BigInteger.Parse("1000000000000000000000000000000"), Amount.NanoToRaw("1"));
        Assert.Equal(BigInteger.Parse("1000000000000000000000000000123"), Amount.NanoToRaw("1.000000000000000000000000000123"));
        Assert.Equal(BigInteger.Parse("55912600000000000000000000000000"), Amount.NanoToRaw("055.91260"));
        Assert.Equal(BigInteger.Parse("1"), Amount.NanoToRaw("0.000000000000000000000000000001"));
    }

    [Fact]
    public void RawToNanoTest()
    {
        Assert.Equal(BigDecimal.Parse("1"), Amount.RawToNano("1000000000000000000000000000000"));
        Assert.Equal(BigDecimal.Parse("1.000000000000000000000000000123"), Amount.RawToNano("1000000000000000000000000000123"));
        Assert.Equal(BigDecimal.Parse("55.9126"), Amount.RawToNano("55912600000000000000000000000000"));
        Assert.Equal(BigDecimal.Parse("0.000000000000000000000000000001"), Amount.RawToNano("1"));
    }
}
