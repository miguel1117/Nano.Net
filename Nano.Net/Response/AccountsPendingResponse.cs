using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nano.Net
{
    public class AccountsPendingResponse
    {
        [JsonProperty("blocks")]
        public Dictionary<string, Dictionary<string, ReceivableBlock>> Blocks { get; set; }
    }
}
