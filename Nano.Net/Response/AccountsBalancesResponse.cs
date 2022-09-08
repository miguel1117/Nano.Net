using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nano.Net;

public class AccountsBalancesResponse
{
    [JsonProperty("balances")]
    public Dictionary<string, BalancePendingPair> Balances { get; set; }
}

public class BalancePendingPair
{
    [JsonProperty("balance")]
    public string Balance { get; set; }
    [JsonProperty("pending")]
    public string Pending { get; set; }
}
