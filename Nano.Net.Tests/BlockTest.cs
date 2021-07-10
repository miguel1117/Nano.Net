using Xunit;

namespace Nano.Net.Tests
{
    public class BlockTest
    {
        [Fact]
        public async void HashTest()
        {
            Account account = Account.FromAddress(Constants.StaticNanoAddress);

            var node = new RpcClient("https://node.shrynode.me/api");
            await node.UpdateAccountAsync(account);

            var sendBlock = Block.CreateSendBlock(account,
                Constants.StaticNanoAddress,
                Amount.FromRaw("5000000000000000000000000000"));

            Assert.Equal("FCD3653C1A4846E5021F9FC6D65699B62DABBDF45A25F41735621F312343797E",
                sendBlock.Hash, 
                true);
        }
    }
}
