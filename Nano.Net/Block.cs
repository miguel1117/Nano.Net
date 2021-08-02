using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chaos.NaCl;
using Newtonsoft.Json;
using static Nano.Net.Utils;

namespace Nano.Net
{
    /// <summary>
    /// Represents a block. Can be created manually or using the CreateSendBlock and CreateReceiveBlock methods. Can be used in relevant RPC commands.
    /// </summary>
    public class Block : BlockBase
    {
        [JsonIgnore] public string Hash => GetHash();

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
                throw new Exception("Insufficient balance.");

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
        /// Create a send block and sign it. Requires the receiving Account object to have all the properties correctly set.
        /// </summary>
        /// <param name="receiver">The Account that will receive the funds.</param>
        /// <param name="blockHash">The receivable block hash.</param>
        /// <param name="amount">The Amount of the receivable block.</param>
        /// <param name="powNonce">The PoW nonce that is required for publishing blocks.</param>
        /// <returns>A signed Block object.</returns>
        public static Block CreateReceiveBlock(Account receiver, string blockHash, Amount amount, string powNonce)
        {
            if (receiver.MissingInformation)
                throw new AccountMissingInformationException();

            var block = new Block()
            {
                Account = receiver.Address,
                Previous = receiver.Frontier,
                Representative = receiver.Representative,
                Balance = (receiver.Balance.Raw + amount.Raw).ToString("0"),
                Link = blockHash,
                Work = powNonce,
                Subtype = BlockSubtype.Receive
            };

            block.Sign(receiver.PrivateKey);

            return block;
        }

        /// <summary><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></summary>
        /// <param name="receiver"><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></param>
        /// <param name="receivableBlock">The ReceivableBlock object to be received.</param>
        /// <param name="powNonce"><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></param>
        /// <returns><inheritdoc cref="CreateReceiveBlock(Account, string, Amount, string)"/></returns>
        public static Block CreateReceiveBlock(Account receiver, ReceivableBlock receivableBlock, string powNonce)
        {
            return CreateReceiveBlock(receiver, receivableBlock.Hash, Amount.FromRaw(receivableBlock.Amount), powNonce);
        }

        private string GetHash()
        {
            var type = new byte[32];
            type[31] = 0x6;

            byte[] balanceBytes = BigInteger.Parse(Balance).ToByteArray(true, true);

            byte[] final = Blake2BHash
            (
                32,
                type,
                PublicKeyFromAddress(Account),
                HexToBytes(Previous),
                PublicKeyFromAddress(Representative),
                new byte[16 - balanceBytes.Length],
                balanceBytes,
                Subtype == BlockSubtype.Send ? PublicKeyFromAddress(Link) : HexToBytes(Link)
            );

            return BytesToHex(final);
        }

        /// <summary>
        /// Sign this block using the provided private key and set its Signature property
        /// </summary>
        /// <param name="privateKey">The private key for this block's account.</param>
        /// <exception cref="Exception">The provided private key doesn't match the public key for this block.</exception>
        public void Sign(byte[] privateKey)
        {
            if (!PublicKeyFromPrivateKey(privateKey)
                .SequenceEqual(PublicKeyFromAddress(Account))) // check if the provided private key matches this block's account
                throw new Exception("The private key doesn't match this block's account");

            Ed25519.KeyPairFromSeed(out byte[] _, out byte[] expandedPrivateKey, privateKey);

            byte[] signatureBytes = Ed25519.Sign(HexToBytes(Hash), expandedPrivateKey);

            Signature = BytesToHex(signatureBytes);
        }
    }
}
