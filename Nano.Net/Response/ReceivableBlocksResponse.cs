using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nano.Net
{
    public class ReceivableBlocksResponse
    {
        [JsonProperty("blocks")] public Dictionary<string, ReceivableBlock> PendingBlocks { get; init; }
    }
}
