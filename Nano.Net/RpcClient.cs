// This is a modified version of the code from https://github.com/Flufd/NanoDotNet/blob/master/NanoDotNet/NanoRpcClient.cs


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nano.Net.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            if (json.Contains("\"error\":"))
            {
                JObject errorMessage = JObject.Parse(json);
                throw new RpcException($"RPC call returned error. Message: {errorMessage["error"]}");
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
        
        // Default node RPC calls
        
        public async Task<AccountInfoResponse> AccountInfoAsync(string address, bool representative = true)
        {
            return await RpcRequestAsync<AccountInfoResponse>(new
            {
                Action = "account_info",
                Account = address,
                Representative = representative
            });
        }

        public async Task<PendingBlocksResponse> PendingBlocksAsync(string address, int count = 5)
        {
            var pendingBlocks = await RpcRequestAsync<PendingBlocksResponse>(new
            {
                Action = "pending",
                Account = address,
                Count = count.ToString(),
                Source = true,
                IncludeOnlyConfirmed = true
            });

            foreach ((string key, PendingBlock value) in pendingBlocks.PendingBlocks)
                value.Hash = key;

            return pendingBlocks;
        }
        
        // Custom calls
        
        public async Task UpdateAccountAsync(Account account)
        {
            AccountInfoResponse accountInfo = await AccountInfoAsync(account.Address);

            account.Frontier = accountInfo.Frontier;
            account.Balance = Amount.FromRaw(accountInfo.Balance);
            account.Representative = accountInfo.Representative;
        }
    }
}
