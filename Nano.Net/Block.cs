using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chaos.NaCl;
using Nano.Net.Response;
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
        
        private const string MissingInformationError = "Not all properties for this account have been set. Please update this account's properties manually or use the RpcClient UpdateAccountAsync method.";

        /// <summary>
        /// Create a send block and sign it. Requires the account object to have all the properties correctly set.
        /// </summary>
        /// <param name="sender">The account that will send the funds.</param>
        /// <param name="receiver">The address that will be receiving the funds.</param>
        /// <param name="amount">The amount of Nano being sent.</param>
        /// <param name="powNonce">The PoW nonce generated from the frontier block hash for this account.</param>
        public static Block CreateSendBlock(Account sender, string receiver, Amount amount, string powNonce)
        {
            if (!sender.Opened)
                throw new UnopenedAccountException();

            if (sender.MissingInformation)
                throw new Exception(MissingInformationError);

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

        public static Block CreateReceiveBlock(Account receiver, string blockHash, Amount amount, string powNonce)
        {
            if (receiver.MissingInformation)
                throw new Exception(MissingInformationError);

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

        public static Block CreateReceiveBlock(Account receiver, PendingBlock pendingBlock, string powNonce)
        {
            return CreateReceiveBlock(receiver, pendingBlock.Hash, Amount.FromRaw(pendingBlock.Amount), powNonce);
        }

        public string GetHash()
        {
            var bytes = new List<byte>();

            var type = new byte[32];
            type[31] = 0x6;
            bytes.AddRange(type);

            byte[] account = PublicKeyFromAddress(Account);
            bytes.AddRange(account);

            byte[] previous = HexToBytes(Previous);
            bytes.AddRange(previous);

            byte[] representative = PublicKeyFromAddress(Representative);
            bytes.AddRange(representative);

            byte[] balanceBytes = BigInteger.Parse(Balance).ToByteArray(true, true);
            bytes.AddRange(new byte[16 - balanceBytes.Length]); // pad balance
            bytes.AddRange(balanceBytes);

            byte[] link = Subtype == BlockSubtype.Send ? PublicKeyFromAddress(Link) : HexToBytes(Link);
            bytes.AddRange(link);

            byte[] final = Blake2BHash(32, bytes.ToArray());

            return BytesToHex(final);
        }

        public void Sign(byte[] privateKey)
        {
            if (!PublicKeyFromPrivateKey(privateKey).SequenceEqual(PublicKeyFromAddress(Account))) // check if the provided private key matches this block's account
                throw new Exception("The private key doesn't match this block's account");
            
            Ed25519.KeyPairFromSeed(out byte[] _, out byte[] expandedPrivateKey, privateKey);

            byte[] signatureBytes = Ed25519.Sign(HexToBytes(Hash), expandedPrivateKey);

            Signature = BytesToHex(signatureBytes);
        }
    }
}
