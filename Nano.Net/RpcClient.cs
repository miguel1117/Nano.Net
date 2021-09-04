// This is a modified version of the code from https://github.com/Flufd/NanoDotNet/blob/master/NanoDotNet/NanoRpcClient.cs


using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        /// <summary>
        /// Create a new RpcClient connected to a local node.
        /// </summary>
        public RpcClient()
        {
            NodeAddress = "http://localhost:7076";
        }

        /// <summary>
        /// Initialize rpc with a specific uri.
        /// </summary>
        /// <param name="nodeUri">Example: Http://192.168.0.1:7076</param>
        public RpcClient(string nodeUri)
        {
            NodeAddress = nodeUri;
        }

        private async Task<T> RpcRequestAsync<T>(object request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request, _jsonSerializerSettings), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(NodeAddress, content);
            string json = await response.Content.ReadAsStringAsync();

            if (json.Contains("\"error\":"))
            {
                JObject errorMessage = JObject.Parse(json);
                throw new RpcException(errorMessage["error"]?.ToString());
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        // Raw node RPC calls

        /// <summary>
        /// Get information about a Nano account.
        /// </summary>
        public async Task<AccountInfoResponse> AccountInfoAsync(string address, bool representative = true)
        {
            try
            {
                return await RpcRequestAsync<AccountInfoResponse>(new
                {
                    Action = "account_info",
                    Account = address,
                    Representative = representative
                });
            }
            catch (RpcException exception)
            {
                if (exception.OriginalError == "Account not found")
                    throw new UnopenedAccountException();
                else
                    throw;
            }
        }
  
        public async Task<AccountHistoryResponse> AccountHistoryAsync(string address, int count = 10)
        {
            return await RpcRequestAsync<AccountHistoryResponse>(new
            {
                Action = "account_history",
                Account = address,
                Count = count
            });
        }
      
        /// <summary>
        /// Generate a work nonce for a hash using the node.
        /// </summary>
        /// <remarks>
        /// WARNING: This command is usually disabled on public nodes. You need to use your own node.
        /// </remarks>
        public async Task<WorkGenerateResponse> WorkGenerateAsync(string hash, string difficulty = null)
        {
            return await RpcRequestAsync<WorkGenerateResponse>(new
            {
                Action = "work_generate",
                Hash = hash,
                Difficulty = difficulty
            });
        }

        /// <summary>
        /// Gets the pending/receivable blocks for an account.
        /// </summary>
        public async Task<ReceivableBlocksResponse> PendingBlocksAsync(string address, int count = 5)
        {
            var pendingBlocks = await RpcRequestAsync<ReceivableBlocksResponse>(new
            {
                Action = "pending",
                Account = address,
                Count = count.ToString(),
                Source = true,
                IncludeOnlyConfirmed = true
            });

            if (pendingBlocks?.PendingBlocks != null)
                foreach ((string key, ReceivableBlock value) in pendingBlocks?.PendingBlocks)
                    value.Hash = key;

            return pendingBlocks;
        }

        public async Task<BlockInfoResponse> BlockInfoAsync(string hash)
        {
            return await RpcRequestAsync<BlockInfoResponse>(new
            {
                Action = "block_info",
                json_block = "true",
                Hash = hash
            });
        }

        /// <summary>
        /// Publishes a Block to the network.
        /// </summary>
        /// <exception cref="Exception">If the block signature or work nonce hasn't been set.</exception>
        public async Task<ProcessResponse> ProcessAsync(Block block)
        {
            if (block.Signature is null)
                throw new IncompleteBlockException("This block hasn't been signed yet.");

            if (block.Work is null)
                throw new IncompleteBlockException("The PoW nonce for this block hasn't been set.");

            return await RpcRequestAsync<ProcessResponse>(new
            {
                Action = "process",
                JsonBlock = true,
                Subtype = block.Subtype,
                Block = block
            });
        }

        public async Task<AccountsBalancesResponse> AccountsBalancesAsync(string[] accounts)
        {
            return await RpcRequestAsync<AccountsBalancesResponse>(new
            {
                Action = "accounts_balances",
                Accounts = accounts
            });
        }

        // Custom calls

        /// <summary>
        /// Update an Account object's properties with relevant information from the network.
        /// </summary>
        public async Task UpdateAccountAsync(Account account)
        {
            AccountInfoResponse accountInfo;

            try
            {
                accountInfo = await AccountInfoAsync(account.Address);
            }
            catch (UnopenedAccountException)
            {
                account.Opened = false;
                account.Frontier = new string('0', 64);
                account.Balance = Amount.FromRaw("0");
                account.Representative = account.Address;
                return;
            }

            account.Opened = true;
            account.Frontier = accountInfo.Frontier;
            account.Balance = Amount.FromRaw(accountInfo.Balance);
            account.Representative = accountInfo.Representative;
        }
    }
}
