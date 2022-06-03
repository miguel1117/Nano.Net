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
        Balance = "364448673",
        Link = "nano_3tjhazni9juaoa8q9rw33nf3f6i45gswhpzrgrbrawxhh7a777ror9okstch",
        Subtype = BlockSubtype.Send
    };

    [Fact]
    public void HashBlock_ValidateHash_ValidHash()
    {
        Assert.Equal("66A580645BEE693103CDE784764043092F7CB966B44C4029576C5CE17C222F40", _testBlock.Hash, true);
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
        
        Assert.Equal("C3E9714F3FAAFE66074F40FB10C70A5A489CE0A222DBEFB7F39AB3432BF426D5A82E56BA24EBEC01CE37B1990A73CBC33E760F57856637778854FEF98E13F500",
            _testBlock.Signature, true);
    }
}
