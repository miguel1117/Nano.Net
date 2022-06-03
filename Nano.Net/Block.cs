using System;
using System.Linq;
using System.Numerics;
using Chaos.NaCl;
using Nano.Net.Extensions;
using Newtonsoft.Json;
using static Nano.Net.Utils;

namespace Nano.Net
{
    /// <summary>
    /// Represents a block. Can be created manually or using the CreateSendBlock and CreateReceiveBlock methods. Can be used in relevant RPC commands.
    /// </summary>
    public class Block : BlockBase
    {
        // these properties aren't included inside the block itself, so they shouldn't be serialized
        [JsonIgnore] public string Hash => GetHash();
        [JsonIgnore] public string Subtype { get; init; }

        /// <summary>
        /// Create a send block and sign it. Requires the sending Account object to have all the properties correctly set.
        /// </summary>
        /// <param name="sender">The Account that will send the funds.</param>
        /// <param name="receiver">The address that will be receiving the funds.</param>
        /// <param name="amount">The amount of Nano being sent.</param>
        /// <param name="powNonce">The PoW nonce generated from the frontier block hash for this account.</param>
        public static Block CreateSendBlock(Account sender, string receiver, Amount amount, string powNonce)
        {
            if (!sender.Opened)
                throw new UnopenedAccountException();

            if (sender.MissingInformation)
                throw new AccountMissingInformationException();

            if (amount.Raw > sender.Balance.Raw)
                throw new InsufficientBalanceException($"Insufficient balance. Balance: {sender.Balance.Nano}; transaction amount: {amount.Nano}");

            var block = new Block()
            {
                Account = sender.Address,
                Previous = sender.Frontier,
                Representative = sender.Representative,
                Balance = (sender.Balance.Raw - amount.Raw).ToString("0"),
                Link = receiver,
                Work = powNonce,
                Subtype = BlockSubtype.Send
            };

            block.Sign(sender.PrivateKey);

            return block;
        }

        /// <summary>
        /// Creates a send block and signs it. Requires the receiving Account object to have all the properties correctly set.
        /// </summary>
        /// <param name="account">The Account that will receive the funds.</param>
        /// <param name="blockHash">The receivable block hash.</param>
        /// <param name="amount">The Amount of funds of the receivable block.</param>
        /// <param name="powNonce">The PoW nonce that is required for publishing blocks.</param>
        /// <returns>A signed Block object.</returns>
        public static Block CreateReceiveBlock(Account account, string blockHash, Amount amount, string powNonce)
        {
            if (account.MissingInformation)
                throw new AccountMissingInformationException();

            var block = new Block()
            {
                Account = account.Address,
                Previous = account.Frontier,
                Representative = account.Representative,
                Balance = (account.Balance.Raw + amount.Raw).ToString("0"),
                Link = blockHash,
                Work = powNonce,
                Subtype = BlockSubtype.Receive
            };

            block.Sign(account.PrivateKey);

            return block;
        }

        /// <summary><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></summary>
        /// <param name="account"><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></param>
        /// <param name="receivableBlock">The object representing the block that will be received.</param>
        /// <param name="powNonce"><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></param>
        /// <returns><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></returns>
        public static Block CreateReceiveBlock(Account account, ReceivableBlock receivableBlock, string powNonce)
        {
            return CreateReceiveBlock(account, receivableBlock.Hash, Amount.FromRaw(receivableBlock.Amount), powNonce);
        }
        
        /// <summary>
        /// Creates a change block, which is used to change the representative of an account. 
        /// </summary>
        /// <param name="account">The Account that will have its representative changed.</param>
        /// <param name="representativeAddress">The address of the representative to change to.</param>
        /// <param name="powNonce">The PoW nonce that is required for publishing blocks.</param>
        /// <returns>A signed Block object.</returns>
        public static Block CreateChangeBlock(Account account, string representativeAddress, string powNonce)
        {
            if (!account.Opened)
                throw new UnopenedAccountException();

            if (account.MissingInformation)
                throw new AccountMissingInformationException();
            
            var block = new Block()
            {
                Account = account.Address,
                Previous = account.Frontier,
                Representative = representativeAddress,
                Balance = account.Balance.Raw.ToString("0"),
                Link = new string('0', 64),
                Work = powNonce,
                Subtype = BlockSubtype.Change
            };
            
            block.Sign(account.PrivateKey);

            return block;
        }

        private string GetHash()
        {
            byte[] balanceBytes = BigInteger.Parse(Balance).ToByteArray();
            if (balanceBytes.Length > 1 && balanceBytes.Last() == 0)
                Array.Resize(ref balanceBytes, balanceBytes.Length - 1); // remove trailing 0 (sign byte)
            balanceBytes = balanceBytes.Reverse().ToArray(); // convert to big endian

            byte[] final = Blake2BHash
            (
                32,
                new byte[31],
                new byte[] { 0x6 },
                PublicKeyFromAddress(Account),
                Previous.HexToBytes(),
                PublicKeyFromAddress(Representative),
                new byte[16 - balanceBytes.Length], // pad balance to ensure it is 16 bytes in total
                balanceBytes,
                Subtype == BlockSubtype.Send ? PublicKeyFromAddress(Link) : Link.HexToBytes()
            );

            return final.BytesToHex();
        }

        /// <summary>
        /// Sign this block using the provided private key and set its Signature property
        /// </summary>
        /// <param name="privateKey">The private key for this block's account.</param>
        /// <exception cref="Exception">The provided private key doesn't match the public key for this block.</exception>
        public void Sign(byte[] privateKey)
        {
            // check if the private key matches this block's account public key
            if (!PublicKeyFromPrivateKey(privateKey).SequenceEqual(PublicKeyFromAddress(Account)))  
                throw new Exception("The private key doesn't match this block's account public key.");

            Ed25519.KeyPairFromSeed(out byte[] _, out byte[] expandedPrivateKey, privateKey);

            byte[] signatureBytes = Ed25519.Sign(Hash.HexToBytes(), expandedPrivateKey);

            Signature = signatureBytes.BytesToHex();
        }
    }
}
