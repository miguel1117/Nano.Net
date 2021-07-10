using System;
using Xunit;

namespace Nano.Net.Tests
{
    public class RpcTest
    {
        private RpcClient _rpcClient = new RpcClient(Constants.NodeAddress);

        [Fact]
        public async void UpdateAccountInfoTest()
        {
            Account account = Account.FromAddress(Constants.StaticNanoAddress);
            await _rpcClient.UpdateAccountAsync(account);
            
            Assert.Equal("3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712", account.Frontier, true);
            Assert.Equal(Amount.FromRaw("15000000000000000000000000000").Raw, account.Balance.Raw);
            Assert.Equal("nano_18shbirtzhmkf7166h39nowj9c9zrpufeg75bkbyoobqwf1iu3srfm9eo3pz", account.Representative, true);
        }
    }
}
