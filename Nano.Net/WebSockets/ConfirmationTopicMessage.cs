using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class ConfirmationTopicMessage : TopicMessage
    {
        [JsonProperty("message")] public Message Message { get; set; }
    }

    public class Message
    {
        [JsonProperty("account")] public string Account { get; set; }

        [JsonProperty("amount")] public string Amount { get; set; }

        [JsonProperty("hash")] public string Hash { get; set; }

        [JsonProperty("confirmation_type")] public string ConfirmationType { get; set; }

        [JsonProperty("block")] public BlockWithSubtype Block { get; set; }
    }

    public class BlockWithSubtype : BlockBase
    {
        // blocks sent for publishing require the subtype outside the block, in the json root, while in websockets the subtype comes inside the block
        [JsonProperty("subtype")] public new string Subtype { get; init; }
    }
}
