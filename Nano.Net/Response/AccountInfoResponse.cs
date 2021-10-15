using Newtonsoft.Json;

namespace Nano.Net
{
    public class AccountInfoResponse
    {
        [JsonProperty("frontier")] 
        public string Frontier { get; set; }
        [JsonProperty("open_block")] 
        public string OpenBlock { get; set; }
        [JsonProperty("representative_block")] 
        public string RepresentativeBlock { get; set; }
        [JsonProperty("balance")] 
        public string Balance { get; set; }
        [JsonProperty("confirmed_balance")] 
        public string ConfirmedBalance { get; set; }
        [JsonProperty("modified_timestamp")] 
        public string ModifiedTimestamp { get; set; }
        [JsonProperty("block_count")] 
        public ulong BlockCount { get; set; }
        [JsonProperty("account_version")] 
        public ulong AccountVersion { get; set; }
        [JsonProperty("confirmation_height")] 
        public ulong ConfirmationHeight { get; set; }
        [JsonProperty("confirmation_height_frontier")] 
        public string ConfirmationHeightFrontier { get; set; }
        [JsonProperty("representative")] 
        public string Representative { get; set; }
        [JsonProperty("weight")] 
        public string Weight { get; set; }
        [JsonProperty("pending")] 
        public string Pending { get; set; }
        [JsonProperty("confirmed_pending")] 
        public string ConfirmedPending { get; set; }
        [JsonProperty("confirmed_height")] 
        public ulong ConfirmedHeight { get; set; }
        [JsonProperty("confirmed_representative")] 
        public string ConfirmedRepresentative { get; set; }
    }
}
