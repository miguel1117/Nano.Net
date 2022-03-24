using System.Numerics;
using Nano.Net.Numbers;
using Xunit;

namespace Nano.Net.Tests;

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
}
