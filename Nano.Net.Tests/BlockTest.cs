using Xunit;

namespace Nano.Net.Tests
{
    public class BlockTest
    {
        [Fact]
        public void HashTest()
        {
            var block = new Block()
            {
                Account = "nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9",
                Previous = "3865BFCD423CE3579C4A7C6010CE763BE4C63964AC06BDA451A63BBCAC9E3712",
                Representative = "nano_18shbirtzhmkf7166h39nowj9c9zrpufeg75bkbyoobqwf1iu3srfm9eo3pz",
                Balance = "0",
                Link = "nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9",
                Subtype = BlockSubtype.Send
            };

            Assert.Equal("BC9E203F6D6254B3446F8F6AD9BF32B9BDC8EC6141F70776B4DF7B7DAFBBDACE",
                block.Hash,
                true);
        }
    }
}
