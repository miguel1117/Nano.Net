using System;
using Xunit;

namespace Nano.Net.Tests
{
    public class RpcTest
    {
        private const string NodeAddress = "https://node.shrynode.me/api";
        private const string StaticNanoAddress = "nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9"; // static burn account used for testing

        private RpcClient _rpcClient = new RpcClient(NodeAddress);

        [Fact]
        public async void UpdateAccountInfoTest()
        {
            Account account = Account.FromAddress("nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9");
            await _rpcClient.UpdateAccountAsync(account);
            
            Assert.Equal("3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712", account.Frontier, true);
            Assert.Equal(Amount.FromRaw("15000000000000000000000000000").Raw, account.Balance.Raw);
            Assert.Equal("nano_18shbirtzhmkf7166h39nowj9c9zrpufeg75bkbyoobqwf1iu3srfm9eo3pz", account.Representative, true);
        }
    }
}
