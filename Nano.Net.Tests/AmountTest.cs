using System.Globalization;
using Xunit;

namespace Nano.Net.Tests
{
    public class AmountTest
    {
        [Fact]
        public void AmountFromRawTest()
        {
            Assert.Equal(
                double.Parse("78.9654", CultureInfo.InvariantCulture),
                Amount.FromRaw("78965400000000000000000000000000").Nano
            );
        }

        [Fact]
        public void AmountFromNanoTest()
        {
            Assert.Equal(
                double.Parse("78965400000000000000000000000000"),
                Amount.FromNano("78.9654").Raw
            );
        }
    }
}
