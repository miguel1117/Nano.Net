# Nano.Net

A .NET library for building projects with [Nano](https://nano.org). Still in development.  
Please feel free to submit any changes, bugfixes or new features using the issues/pull requests feature here on
GitHub.  
This project follows Semantic Versioning.

## Features/Roadmap

* [x] Seed generation and derivation
* [x] Keypair and address generation
* [x] Local block signing
* [x] Unit conversion
* [x] RPC client for interacting with the network
* [x] Custom address prefix support (e.g., ban_)
* [x] WebSockets support

## Requirements

* .NET Core 3.1 or .NET 5 and higher

## Installation

You can either:

* Install the package [from Nuget](https://www.nuget.org/packages/Nano.Net/).
* Include the Nano.Net project in your project's solution and add a reference to it in your projects.


## Usage

**Using the RPC client**

```c#
var rpcClient = new RpcClient(nodeAddress);

AccountInfoResponse accountInfo = await rpcClient.AccountInfoAsync("nano_3r9rdhbipf9xsnpxdhf7h7kebo8iyfefc9s3bcx4racody5wubz1y1kzaon9");
Console.WriteLine($"Balance (raw): {accountInfo.Balance}");
Console.WriteLine($"Representative: {accountInfo.Representative}");
```

**Account creation and seed derivation**

```c#
// There are Account constructors overloads available that accept
// private keys/seeds encoded as hex strings and byte arrays.

// Create a new Account object with a randomly generated private key
Account account1 = new Account();

// Generate a random hex seed and create an Account using it and an index.
string randomSeed = Utils.GenerateSeed();
Account account2 = new Account(randomSeed, 5);

// Create an Account from a private key.
Account account3 = new Account(privateKey: "A066701E0641E524662E3B7F67F98A248C300017BAA8AA0D91A95A2BCAF8D4D8");
```

**Account creation with a custom address prefix**

```c#
// Specify the prefix.
Account account = new Account("ban");
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

**Sending and receiving a transaction**

* Important: Make sure the account balance is up to date before sending a transaction, or you could end up sending more
  than you wanted.  
  For more
  information [check this warning from the official docs](https://docs.nano.org/integration-guides/key-management/?h=bip#:~:text=Warning,account_info%20RPC%20call.)
  .

```c#
var rpcClient = new RpcClient(nodeAddress);
Account account = new Account(privateKey: "A066701E0641E524662E3B7F67F98A248C300017BAA8AA0D91A95A2BCAF8D4D8");
            
// Sets the balance, representative and frontier for this account from a node. Can also be set manually.
await rpcClient.UpdateAccountAsync(account);

// Generate a PoW nonce using the node rpcClient is connected to.
// Note: public nodes usually have the work generation rpc command disabled, so you will need to use your own node.
WorkGenerateResponse workResponse = await rpcClient.WorkGenerateAsync(account.Frontier);
string pow = workResponse.Work;
            
// Creates a block and automatically sign it. The PoW nonce has to be obtained externally.
Block sendBlock = Block.CreateSendBlock(account,
    "nano_3tjhazni9juaoa8q9rw33nf3f6i45gswhpzrgrbrawxhh7a777ror9okstch",
    Amount.FromRaw("1"), 
    pow);
            
// Publish the block to the network.
await rpcClient.ProcessAsync(sendBlock);

// Receive a transaction
await rpcClient.UpdateAccountAsync(account); // account data should be updated before publishing a block
workResponse = await rpcClient.WorkGenerateAsync(account.Frontier);
pow = workResponse.Work;

Block receiveBlock = Block.CreateReceiveBlock(account, "BCB03523591F42792EAF315676FF944D3530C0F1A38F55066BDF26EA15B7073A", 
    Amount.FromRaw("1"), pow); // you can also get pending blocks for an account using the rpc client and use a PendingBlock object as an argument.
await rpcClient.ProcessAsync(receiveBlock);
```

**WebSockets usage**

* Note: if you're using a public node you should be aware that some of them may:
    * Have WebSocket support disabled.
    * Behave differently than the "vanilla" nano node behaviour. 
      In those cases, NanoWebSocketClient may not work as expected.

```c#
// Connect to a websocket enpoint.
var w = new NanoWebSocketClient(nodeAddress);

// Subscribe to a topic. Some topics may have options. Topics can be found in the WebsSockets/Topics directory.
w.Subscribe(new ConfirmationTopic());

// Subscribe to the catch-all event, which will send messages for all topics the client receives.
w.Message += (client, content) => { Console.WriteLine(content); };

// Subscribe to the Confirmation topic event.
w.Confirmation += (client, message) => { Console.WriteLine(message.Message.Amount); };

// Don't forget to run Start() to actually start receiving the messages.
await w.Start();
```

## Acknowledgements

* [NanoDotNet](https://github.com/Flufd/NanoDotNet) for bits of code, including the Nano base32 implementation
* [Chaos.NaCl](https://github.com/CodesInChaos/Chaos.NaCl) for the original C# implementation of ED25519 signing and
  keypair generation
* [BLAKE2](https://github.com/BLAKE2/BLAKE2) for the C# implementation of Blake2B
* [BigDecimal](https://github.com/AdamWhiteHat/BigDecimal) for an arbitrary precision decimal for .NET

## Donations

This library is 100% free, but donations are accepted at the address below. Any amount is appreciated.
`nano_3h71hnsc5asdsd9qfetxwr6do7ebkyef6ufwbybfbh3z7itts6wym5b4tg7x`
