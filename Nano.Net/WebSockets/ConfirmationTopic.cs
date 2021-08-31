using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class ConfirmationTopic : Topic
    {
        public string ConfirmationType { get; set; }
        public string[] Accounts { get; set; }
        public bool? IncludeBlock { get; set; }
        public bool? IncludeElectionInfo { get; set; }

        public ConfirmationTopic()
        {
        }

        public ConfirmationTopic(string confirmationType = null, string[] accounts = null, bool? includeBlock = null, bool? includeElectionInfo = null)
        {
            ConfirmationType = confirmationType;
            Accounts = accounts;
            IncludeBlock = includeBlock;
            IncludeElectionInfo = includeElectionInfo;
        }

        public override string GetSubscribeCommand()
        {
            return JsonConvert.SerializeObject(new
            {
                Action = "subscribe",
                Topic = "confirmation",
                Options = new { ConfirmationType, Accounts, IncludeBlock, IncludeElectionInfo }
            }, JsonSerializerSettings);
        }
    }
}
