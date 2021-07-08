// This is a modified version of the code from https://github.com/Flufd/NanoDotNet/blob/master/NanoDotNet/NanoRpcClient.cs


using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nano.Net.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nano.Net
{
    public class RpcClient
    {
        public string NodeAddress { get; }

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public RpcClient(string nodeAddress)
        {
            NodeAddress = nodeAddress;
        }

        private async Task<T> RpcRequestAsync<T>(object request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request, _jsonSerializerSettings), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(NodeAddress, content);
            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<AccountInfoResponse> AccountInfoAsync(Account account, bool representative = true)
        {
            return await RpcRequestAsync<AccountInfoResponse>(new
            {
                Action = "account_info",
                Account = account.Address,
                Representative = representative
            });
        }
    }
}
