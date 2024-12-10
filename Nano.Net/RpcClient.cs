// This is a modified version of the code from https://github.com/Flufd/NanoDotNet/blob/master/NanoDotNet/NanoRpcClient.cs

using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Nano.Net;

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
    public RpcClient() : this("http://localhost:7076")
    {
    }

    /// <summary>
    /// Create a new RpcClient object with the specific URI.
    /// </summary>
    /// <param name="nodeUri">Example: http://192.168.0.1:7076</param>
    public RpcClient(string nodeUri)
    {
        NodeAddress = nodeUri;
    }

    public async Task<T> RpcRequestAsync<T>(object request)
    {
        string serializedBlock = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
        var content = new StringContent(serializedBlock, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(NodeAddress, content).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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
    public async Task<AccountInfoResponse> AccountInfoAsync(string address, bool representative = true, bool pending = true, bool weight = true, bool confirmed = true)
    {
        try
        {
            return await RpcRequestAsync<AccountInfoResponse>(new
            {
                Action = "account_info",
                Account = address,
                Representative = representative,
                Pending = pending,
                Weight = weight,
                IncludeConfirmed = confirmed
            }).ConfigureAwait(false);
        }
        catch (RpcException exception)
        {
            if (exception.OriginalError == "Account not found")
                throw new UnopenedAccountException();
            else
                throw;
        }
    }

    public async Task<AccountsFrontiersResponse> AccountsFrontiersAsync(string[] accounts)
    {
        return await RpcRequestAsync<AccountsFrontiersResponse>(new
        {
            Action = "accounts_frontiers",
            Accounts = accounts
        }).ConfigureAwait(false);
    }

    public async Task<AccountHistoryResponse> AccountHistoryAsync(string address, int count = 10)
    {
        return await RpcRequestAsync<AccountHistoryResponse>(new
        {
            Action = "account_history",
            Account = address,
            Count = count
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Generate a work nonce for a hash using the node.
    /// </summary>
    /// <remarks>
    /// This command is often disabled on public nodes.
    /// You may need to run your own node or <a href="https://github.com/nanocurrency/nano-work-server">nano-work-server</a>.
    /// </remarks>
    public async Task<WorkGenerateResponse> WorkGenerateAsync(string hash, string difficulty = null)
    {
        return await RpcRequestAsync<WorkGenerateResponse>(new
        {
            Action = "work_generate",
            Hash = hash,
            Difficulty = difficulty
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Check whether work is valid for block.
    /// </summary>
    /// <returns>
    /// A WorkValidateResponse instance.
    /// WorkValidateResponse.ValidAll is true if the work is valid at the current network difficulty (work can be used for any block).
    /// WorkValidateResponse.ValidReceive is true if the work is valid for use in a receive block.
    /// </returns>
    public async Task<WorkValidateResponse> ValidateWorkAsync(string work, string hash)
    {
        return await RpcRequestAsync<WorkValidateResponse>(new
        {
            Action = "work_validate",
            Work = work,
            Hash = hash
        }).ConfigureAwait(false);
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
        }).ConfigureAwait(false);

        if (pendingBlocks?.PendingBlocks != null)
            foreach (var block in pendingBlocks.PendingBlocks)
                block.Value.Hash = block.Key;

        return pendingBlocks;
    }

    public async Task<BlockInfoResponse> BlockInfoAsync(string hash)
    {
        return await RpcRequestAsync<BlockInfoResponse>(new
        {
            Action = "block_info",
            json_block = "true",
            Hash = hash
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Publishes a Block to the network.
    /// </summary>
    /// <exception cref="IncompleteBlockException">If the block signature or work nonce hasn't been set.</exception>
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
        }).ConfigureAwait(false);
    }

    public async Task<AccountsBalancesResponse> AccountsBalancesAsync(string[] accounts)
    {
        return await RpcRequestAsync<AccountsBalancesResponse>(new
        {
            Action = "accounts_balances",
            Accounts = accounts
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a list of confirmed block hashes which have not yet been received by these accounts
    /// </summary>
    public async Task<AccountsPendingResponse> AccountsPendingAsync(string[] accounts, int count = 5)
    {
        var receivableBlocks = await RpcRequestAsync<AccountsPendingResponse>(new
        {
            Action = "accounts_pending",
            Accounts = accounts,
            Source = true,
            Count = count
        }).ConfigureAwait(false);

        // AccountsPendingResponse.Blocks is null if the requested accounts have no receivable blocks
        receivableBlocks.Blocks ??= new Dictionary<string, Dictionary<string, ReceivableBlock>>();

        foreach (var address in receivableBlocks.Blocks)
        {
            if (address.Value is null)
                continue;

            foreach (var block in address.Value)
                block.Value.Hash = block.Key;
        }

        return receivableBlocks;
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
            accountInfo = await AccountInfoAsync(account.Address).ConfigureAwait(false);
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
