using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class NewUnconfirmedBlockTopic : Topic
    {
        public override string GetSubscribeCommand()
        {
            return JsonConvert.SerializeObject(new
            {
                Action = "subscribe",
                Topic = "new_unconfirmed_block"
            });
        }
    }
}
