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

        [Fact]
        public void NewAccountTest()
        {
            const string seed = "7B58BB18E887F5146DD46ACAD89E5139D363F5F4A50DE9A0B465217A51D1BFDF";

            var account = new Account(seed, 63);
            Assert.Equal(Utils.HexToBytes("003D0478B4E3F01889A0E9BB149C701E90EFC0E1BC1702671AA9790F71FED69E"), account.PrivateKey);
            Assert.Equal(Utils.HexToBytes("83778F31E96011CC3C132680A512FECC24E33174F4FB8443A583D6B30F744AAA"), account.PublicKey);
            Assert.Equal("nano_31uqjwrykr1jsiy38bn1nnbhxm36werqbx9uij3td1yppe9qakocutbzzmd9", account.Address);
        }
    }
}
