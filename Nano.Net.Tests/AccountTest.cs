using Xunit;

namespace Nano.Net.Tests
{
    public class AccountTest
    {
        [Fact]
        public void AccountFromSeedTest()
        {
            const string seed = "61C347364D480205C923DCB250BBA2F8604D62BCDE186E88A5B83E06627C8457";
            
            var account = Account.FromSeed(seed, 15);
            Assert.Equal("nano_1tgoapei5niqht3ikqkfpu8zsj5y8spa5i393i9kia53dpp5ikqjwgmahan4",account.Address);
        }
        
        [Fact]
        public void AccountFromPrivateKeyTest()
        {
            const string privateKey = "066A6E2AC1E75E18A6D0A9586348245FB302D9ECE17D4557EAF2AEF8C6E0DA90";
            
            var account = Account.FromPrivateKey(privateKey);
            Assert.Equal("nano_3fkaipau6kpfejzsr6c4itkm3zkf3jqd9dxkccr3so5fbypop4uu9f4fcacz",account.Address);
        }
    }
}
