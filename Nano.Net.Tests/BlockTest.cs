using System;
using Nano.Net.Extensions;
using Xunit;

namespace Nano.Net.Tests;

public class BlockTest
{
    private readonly Block _testBlock = new Block()
    {
        Account = "nano_3tjhazni9juaoa8q9rw33nf3f6i45gswhpzrgrbrawxhh7a777ror9okstch",
        Previous = "3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712",
        Representative = "nano_18shbirtzhmkf7166h39nowj9c9zrpufeg75bkbyoobqwf1iu3srfm9eo3pz",
        Balance = "33022",
        Link = "nano_3tjhazni9juaoa8q9rw33nf3f6i45gswhpzrgrbrawxhh7a777ror9okstch",
        Subtype = BlockSubtype.Send
    };

    [Fact]
    public void HashBlock_ValidateHash_ValidHash()
    {
        Assert.Equal("F19FCBB3145E6A54D6390016245B2FDDCD864DDD5CA04961F05B6F56C3FD28C3", _testBlock.Hash, true);
    }

    [Fact]
    public void SignBlock_WrongKey_ThrowsException()
    {
        Assert.Throws<Exception>(() => _testBlock.Sign("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".HexToBytes()));
    }

    [Fact]
    public void SignBlock_ValidateSignature_ValidSignature()
    {
        _testBlock.Sign("A066701E0641E524662E3B7F67F98A248C300017BAA8AA0D91A95A2BCAF8D4D8".HexToBytes());
        
        Assert.Equal("E1A909D04D2E45199A2F17DF31CFE28203A5593173FB67F12ACB6C26E0B97712542BE472116D3CBC41B480C74CAB95FCB592CE73A21457B6ACF33100E706E500",
            _testBlock.Signature, true);
    }
}
