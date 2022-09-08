using Newtonsoft.Json;

namespace Nano.Net;

public class AccountHistoryResponse
{
    [JsonProperty("account")]
    public string Account { get; set; }
    [JsonProperty("history")]
    public HistoryBlock[] History { get; set; }
    [JsonProperty("previous")]
    public string Previous { get; set; }
}

public class HistoryBlock
{
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("account")]
    public string Account { get; set; }
    [JsonProperty("amount")]
    public string Amount { get; set; }
    [JsonProperty("local_timestamp")]
    public string LocalTimestamp { get; set; }
    [JsonProperty("height")]
    public ulong Height { get; set; }
    [JsonProperty("hash")]
    public string Hash { get; set; }
}
