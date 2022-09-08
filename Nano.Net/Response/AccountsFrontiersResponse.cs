using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nano.Net;

public class AccountsFrontiersResponse
{
    [JsonProperty("frontiers")]
    public Dictionary<string, string> Frontiers { get; set; }
}
