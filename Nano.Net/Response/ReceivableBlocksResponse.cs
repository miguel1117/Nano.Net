using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nano.Net;

public class ReceivableBlocksResponse
{
    [JsonProperty("blocks")]
    public Dictionary<string, ReceivableBlock> PendingBlocks { get; init; }
}

public class ReceivableBlock
{
    [JsonProperty("hash")]
    public string Hash { get; set; }

    [JsonProperty("amount")]
    public string Amount { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }
}
