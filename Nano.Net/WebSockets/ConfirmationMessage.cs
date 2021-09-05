using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class ConfirmationMessage : Message
    {
        [JsonProperty("message")]
        public ConfirmationMessageContents Contents { get; set; }
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

        [JsonProperty("block")]
        public BlockWithSubtype Block { get; set; }
    }
}
