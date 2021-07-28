using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nano.Net
{
    public class PendingBlocksResponse
    {
        [JsonProperty("blocks")] public Dictionary<string, PendingBlock> PendingBlocks { get; init; }
    }
}
