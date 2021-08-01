# Nano.Net
A .NET library for [Nano](https://nano.org).
Still in development. Please feel free to submit any changes, bugfixes or new features with pull requests.


## Features/Roadmap
* [x] Seed generation and derivation
* [x] Keypair and address generation
* [x] Local block signing
* [x] Unit conversion
* [x] RPC client for interacting with the network
* [ ] Improve documentation


## Usage
**Using the RPC client**
```c#
var rpcClient = new RpcClient("NODE_ADDRESS");

AccountInfoResponse accountInfo = await rpcClient.AccountInfoAsync("nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9");
Console.WriteLine($"Balance (raw): {accountInfo.Balance}");
Console.WriteLine($"Representative: {accountInfo.Representative}");
```

**Account creation and seed derivation**
```c#
// Generate a random seed and create an Account using it and an index.
string randomSeed = Utils.GenerateSeed();
Account account1 = Account.FromSeed(randomSeed, 5);

// Create an Account from a private key
Account account2 = Account.FromPrivateKey("A066701E0641E524662E3B7F67F98A248C300017BAA8AA0D91A95A2BCAF8D4D8");
```

**Key and address conversion**
```c#
// Private key from a seed and index
byte[] privateKey = Utils.DerivePrivateKey("949DD42FB17350D7FDDEDFFBD44CB1D4DF977026E715E0C91C5A62FB6CA72716", index: 5);
            
// Public key from private key
byte[] publicKey = Utils.PublicKeyFromPrivateKey(privateKey);
            
// Convert from address to public key or in reverse
string address = Utils.AddressFromPublicKey(publicKey);
publicKey = Utils.PublicKeyFromAddress(address);
```

**Sending and receiving  a transaction**  
* Important: Make sure the account balance is up to date before sending a transaction,
  or you could end up sending more than you wanted.
  For more information [check this warning from the official docs](https://docs.nano.org/integration-guides/key-management/?h=bip#:~:text=Warning,account_info%20RPC%20call.).
```c#
var rpcClient = new RpcClient("NODE_ADDRESS");
Account account = Account.FromPrivateKey("A066701E0641E524662E3B7F67F98A248C300017BAA8AA0D91A95A2BCAF8D4D8");
            
// Sets the balance, representative and frontier for this account from a node. Can also be set manually.
await rpcClient.UpdateAccountAsync(account);
            
// Creates a block and automatically sign it. The PoW nonce has to be obtained externally. Unit conversion isn't implemented yet.
var sendBlock = Block.CreateSendBlock(account,
    "nano_3tjhazni9juaoa8q9rw33nf3f6i45gswhpzrgrbrawxhh7a777ror9okstch",
    Amount.FromRaw("1"), 
    "POW_NONCE");
            
// Publish the block to the network.
await rpcClient.ProcessAsync(sendBlock);

// Receive a transaction
var receiveBlock = Block.CreateReceiveBlock(account, "PENDING_BLOCK_HASH", Amount.FromRaw("1"), "POW_NONCE"); // You can also get pending blocks for an account using the rpc client and use a PendingBlock object as an argument.

await rpcClient.ProcessAsync(receiveBlock);
```


## Acknowledgements
* [NanoDotNet](https://github.com/Flufd/NanoDotNet) for bits of code, including the Nano base32 implementation
* [Chaos.NaCl](https://github.com/CodesInChaos/Chaos.NaCl) for the original C# implementation of ED25519 signing and keypair generation
* [BLAKE2](https://github.com/BLAKE2/BLAKE2) for the C# implementation of Blake2B
* [BigDecimal](https://github.com/AdamWhiteHat/BigDecimal) for an arbitrary precision decimal for .NET

## Donations
This library is 100% free, but donations are accepted at the address below. Any amount is appreciated.
`nano_168owitqjg7nhg1jqcqf9q4uqgtmcp5h9xtydh6ybz7zqa9mp197draz4azs`
