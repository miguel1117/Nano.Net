using System;
using Nano.Net.Response;
using Xunit;

namespace Nano.Net.Tests
{
    public class RpcTest
    {
        private readonly RpcClient _rpcClient = new RpcClient(Constants.NodeAddress);

        [Fact]
        public async void UpdateAccountInfoTest()
        {
            AccountInfoResponse accountInfo = await _rpcClient.AccountInfoAsync(Constants.ReferenceAccount);

            Assert.Equal("3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712", accountInfo.Frontier, true);
            Assert.Equal("15000000000000000000000000000", accountInfo.Balance, true);
            Assert.Equal("nano_18shbirtzhmkf7166h39nowj9c9zrpufeg75bkbyoobqwf1iu3srfm9eo3pz", accountInfo.Representative, true);
        }

        [Fact]
        public async void UnopenedAccountTest()
        {
            await Assert.ThrowsAsync<UnopenedAccountException>(async () =>
                await _rpcClient.AccountInfoAsync("nano_3cxdogotqcxdogotqcxdogotqcxdogotqcxdogotqcxdogotqcxdqq8mbd6p"));
        }

        [Fact]
        public async void RpcErrorTest()
        {
            await Assert.ThrowsAsync<RpcException>(async () => await _rpcClient.AccountInfoAsync("0"));
        }

        [Fact]
        public async void PendingBlockTest()
        {
            PendingBlocksResponse pendingBlocks = await _rpcClient.PendingBlocksAsync(Constants.ReferenceAccount);
            Assert.NotEmpty(pendingBlocks.PendingBlocks);
        }

        [Fact]
        public async void WorkGenerateTest()
        {
            /*const string hash = "3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712";
            WorkGenerateResponse workGenerate = await _rpcClient.WorkGenerateAsync(hash);

            Assert.Equal(hash, workGenerate.Hash);
            Assert.NotNull(workGenerate.Work);*/
        }

        [Fact]
        public async void SendBlockTest()
        {
            /*Account account = Account.FromPrivateKey();
            await _rpcClient.UpdateAccountAsync(account);
            
            var block = Block.CreateSendBlock();

            await _rpcClient.ProcessAsync(block);*/
        }

        [Fact]
        public async void ReceiveBlockTest()
        {
            /*Account account = Account.FromPrivateKey();
            await _rpcClient.UpdateAccountAsync(account);
            
            var block = Block.CreateReceiveBlock();

            await _rpcClient.ProcessAsync(block);*/
        }
    }
}
