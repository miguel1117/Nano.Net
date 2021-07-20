namespace Nano.Net
{
    public static class BlockSubtype // a class is used here instead of an enum because the block subtype needs to be included in the process command
    {
        public const string Send = "send";
        public const string Receive = "receive";
        public const string Change = "change";
    }
}
