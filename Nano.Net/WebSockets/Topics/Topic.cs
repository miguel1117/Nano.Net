using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nano.Net.WebSockets
{
    public abstract class Topic
    {
        protected static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public abstract string GetSubscribeCommand();
    }
}
