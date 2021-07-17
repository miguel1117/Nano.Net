using System;
using Nano.Net.Response;
using Xunit;

namespace Nano.Net.Tests
{
    public class RpcTest
    {
        private RpcClient _rpcClient = new RpcClient(Constants.NodeAddress);

        [Fact]
        public async void UpdateAccountInfoTest()
        {
            AccountInfoResponse accountInfo = await _rpcClient.AccountInfoAsync(Constants.ReferenceAccount);
            
            Assert.Equal("3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712", accountInfo.Frontier, true);
            Assert.Equal("15000000000000000000000000000", accountInfo.Balance, true);
            Assert.Equal("nano_18shbirtzhmkf7166h39nowj9c9zrpufeg75bkbyoobqwf1iu3srfm9eo3pz", accountInfo.Representative, true);
        }

        [Fact]
        public async void PendingBlockTest()
        {
            PendingBlocksResponse pendingBlocks = await _rpcClient.PendingBlocksAsync(Constants.ReferenceAccount);
            Assert.NotNull(pendingBlocks.Blocks);
        }
        
        [Fact]
        public async void BlockInfoTest()
        {
            BlockInfoResponse blockInfo = await _rpcClient.BlockInfoAsync("75F0B821DE3B25908755520117660E1297DDEA774DEC817FAA2C27221442403A");
            
            Assert.Equal("nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9", blockInfo.Contents.Account, true);
            Assert.NotNull(blockInfo.Contents.Previous);
        }
    }
}
