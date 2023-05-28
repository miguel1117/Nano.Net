using System.Numerics;
using Nano.Net.Numbers;
using Xunit;

namespace Nano.Net.Tests;

// BigInteger shouldn't be used in the method's arguments, because currently xunit seems to crash when
// trying to implicitly convert an int to a BigInteger.

public class AmountTest
{
    [Theory]
    [InlineData("1", "1000000000000000000000000000000")]
    [InlineData("1.000000000000000000000000000123", "1000000000000000000000000000123")]
    [InlineData("055.91260", "55912600000000000000000000000000")]
    [InlineData("0.000000000000000000000000000001", "1")]
    public void ConvertUnits_NanoToRaw_ValidResult(string rawAmount, string result)
    {
        Assert.Equal(BigInteger.Parse(result), Amount.NanoToRaw(rawAmount));
    }

    [Theory]
    [InlineData("1000000000000000000000000000000", "1")]
    [InlineData("1000000000000000000000000000123", "1.000000000000000000000000000123")]
    [InlineData("55912600000000000000000000000000", "055.91260")]
    [InlineData("1", "0.000000000000000000000000000001")]
    public void ConvertUnits_RawToNano_ValidResult(string rawAmount, string result)
    {
        Assert.Equal(BigDecimal.Parse(result), Amount.RawToNano(rawAmount));
    }

    [Theory]
    [InlineData(1000453, 3573521, 4573974)]
    public void AmountMath_Sum_ValidResult(int a, int b, int result)
    {
        Assert.Equal(result, (new Amount(a) + new Amount(b)).Raw);
    }

    [Theory]
    [InlineData(4573974, 3573521, 1000453)]
    public void AmountMath_Subtract_ValidResult(int a, int b, int result)
    {
        Assert.Equal(result, (new Amount(a) - new Amount(b)).Raw);
    }

    [Fact]
    public void AmountComparison_SameValue_Equal()
    {
        Assert.Equal(new Amount(15), new Amount(15));
    }

    [Fact]
    public void AmountComparison_DifferentValues_Different()
    {
        Assert.NotEqual(new Amount(15), new Amount(25));
    }
}
