using Newtonsoft.Json;

namespace Nano.Net.WebSockets;

public class NewUnconfirmedBlockTopic : Topic
{
    public override string Name => "new_unconfirmed_block";

    public override object GetOptions()
    {
        return null;
    }
}
