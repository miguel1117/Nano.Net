using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class ConfirmationMessage : IMessage
    {
        [JsonProperty("topic")]
        public string Topic { get; init; }

        [JsonProperty("time")]
        public string Time { get; init; }

        [JsonProperty("message")]
        public ConfirmationMessageContents Message { get; set; }
    }

    public class ConfirmationMessageContents
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("confirmation_type")]
        public string ConfirmationType { get; set; }

        [JsonProperty("election_info")]
        public ElectionInfo ElectionInfo { get; set; }

        [JsonProperty("block")]
        public BlockWithSubtype Block { get; set; }
    }

    public class ElectionInfo
    {
        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("tally")]
        public string Tally { get; set; }

        [JsonProperty("request_count")]
        public long RequestCount { get; set; }

        [JsonProperty("blocks")]
        public long Blocks { get; set; }

        [JsonProperty("voters")]
        public long Voters { get; set; }
    }
}
