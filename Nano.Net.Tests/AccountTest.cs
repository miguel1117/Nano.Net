using Nano.Net.Extensions;
using Xunit;

namespace Nano.Net.Tests;

public class AccountTest
{
    [Fact]
    public void NewAccount_FromPrivateKey_ValidAddress()
    {
        var account = new Account(privateKey: "066A6E2AC1E75E18A6D0A9586348245FB302D9ECE17D4557EAF2AEF8C6E0DA90");
        Assert.Equal("nano_3fkaipau6kpfejzsr6c4itkm3zkf3jqd9dxkccr3so5fbypop4uu9f4fcacz", account.Address);
    }

    [Fact]
    public void NewAccount_FromSeed_ValidProperties()
    {
        var account = new Account("7B58BB18E887F5146DD46ACAD89E5139D363F5F4A50DE9A0B465217A51D1BFDF", 63);
        
        Assert.Equal("003D0478B4E3F01889A0E9BB149C701E90EFC0E1BC1702671AA9790F71FED69E".HexToBytes(), account.PrivateKey);
        Assert.Equal("83778F31E96011CC3C132680A512FECC24E33174F4FB8443A583D6B30F744AAA".HexToBytes(), account.PublicKey);
        Assert.Equal("nano_31uqjwrykr1jsiy38bn1nnbhxm36werqbx9uij3td1yppe9qakocutbzzmd9", account.Address);
    }
}
