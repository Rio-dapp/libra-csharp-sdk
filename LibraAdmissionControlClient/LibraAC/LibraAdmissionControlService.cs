using Grpc.Core;
using LibraAdmissionControlClient.Dtos;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static AdmissionControl.AdmissionControl;
using Types;
using System;
using Google.Protobuf;

using System.Security.Cryptography;
using NSec.Cryptography;
using Waher.Security.SHA3;
using LibraAdmissionControlClient.Utilityes;
using LibraAdmissionControlClient.LCS.LCSTypes;
using LibraAdmissionControlClient.LCS;

namespace LibraAdmissionControlClient
{
    /// <summary>
    /// Main Class
    /// </summary>
    public class LibraAdmissionControlService
    {
        public ChannelState State
        {
            get
            {
                if (_channel == null)
                {
                    return ChannelState.Shutdown;
                }
                return _channel.State;
            }
        }

        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
        }

        private AdmissionControlClient _client;
        public AdmissionControlClient Client
        {
            get { return _client; }
        }

        /// <summary>
        /// The Main method Initialize
        /// </summary>
        /// <param name="host">ac.testnet.libra.org</param>
        /// <param name="port">8000 or 30307</param>
        public LibraAdmissionControlService(string host, int port)
        {
            _channel = new Channel(host, port, ChannelCredentials.Insecure);
            _client = new AdmissionControlClient(_channel);
        }

        public async Task<Types.AccountStateWithProof> GetAccountInfoAsync(
            string address)
        {
            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var asr = new Types.GetAccountStateRequest();
            asr.Address = Google.Protobuf.ByteString.CopyFrom(
                address.HexStringToByteArray());
            requestItem.GetAccountStateRequest = asr;
            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);

            var result = await _client.UpdateToLatestLedgerAsync(
                updateToLatestLedgerRequest, new Metadata());

            if (result == null ||
                result.ResponseItems == null ||
                result.ResponseItems != null && !result.ResponseItems.Any())
                throw new System.Exception("No such address.");

            var firest = result.ResponseItems.FirstOrDefault();

            if (firest.GetAccountStateResponse.AccountStateWithProof.Blob == null)
                throw new System.Exception("AccountStateWithProof Blob is empty.");

            return firest.GetAccountStateResponse.AccountStateWithProof;
        }

        public async Task<Types.TransactionListWithProof> GetTransactionsAsync(
            ulong startVersion, ulong limit)
        {
            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var tansactionRequest = new GetTransactionsRequest();
            requestItem.GetTransactionsRequest = tansactionRequest;
            tansactionRequest.StartVersion = startVersion;

            tansactionRequest.Limit = limit;
           // tansactionRequest.FetchEvents = true;

            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);
            var result = await _client.UpdateToLatestLedgerAsync(
                updateToLatestLedgerRequest, new Metadata());
            //Console.WriteLine("result = " + result.ResponseItems.Count);
            //Console.WriteLine("result = " + result.ResponseItems.FirstOrDefault());

            List<CustomRawTransaction> retList = new List<CustomRawTransaction>();
            foreach (var item in result.ResponseItems)
                return item.GetTransactionsResponse.TxnListWithProof;

            return null;
        }

        public async Task<Types.SignedTransactionWithProof> 
            GetTransactionsBySequenceNumberAsync(
           string address, ulong sequenceNumber)
        {
            var updateToLatestLedgerRequest = new Types.UpdateToLatestLedgerRequest();
            var requestItem = new Types.RequestItem();
            var tansactionRequest = new GetAccountTransactionBySequenceNumberRequest();
            requestItem.GetAccountTransactionBySequenceNumberRequest 
                = tansactionRequest;
            tansactionRequest.Account =
                Google.Protobuf.ByteString.CopyFrom(address.HexStringToByteArray());
            tansactionRequest.SequenceNumber = sequenceNumber;
            tansactionRequest.FetchEvents = true;

            updateToLatestLedgerRequest.RequestedItems.Add(requestItem);
            var result = await _client.UpdateToLatestLedgerAsync(
                updateToLatestLedgerRequest, new Metadata());

            foreach (var item in result.ResponseItems)
                return item.GetAccountTransactionBySequenceNumberResponse
                    .SignedTransactionWithProof;

            return null;
        }


        public async Task<AdmissionControl.SubmitTransactionResponse>
            SendTransactionAsync(
            byte[] privateKey, RawTransactionLCS rawTransaction)
        {
            var bytesTrx = LCSCore.LCSerialize(rawTransaction);
            LibraHasher libraHasher = new LibraHasher(EHashType.RawTransaction);
            var hash = libraHasher.GetHash(bytesTrx);

            var key = Key.Import(SignatureAlgorithm.Ed25519, privateKey,
                KeyBlobFormat.RawPrivateKey);
            AdmissionControl.SubmitTransactionRequest req =
                new AdmissionControl.SubmitTransactionRequest();
            
            req.SignedTxn = new SignedTransaction();

            List<byte> retArr = new List<byte>();
            retArr = retArr.Concat(bytesTrx).ToList();
            retArr = retArr.Concat(
                LCSCore.LCSerialize(key.Export(KeyBlobFormat.RawPublicKey))).ToList();
            var sig = SignatureAlgorithm.Ed25519.Sign(key, hash);
            retArr = retArr.Concat(LCSCore.LCSerialize(sig)).ToList();
            req.SignedTxn.SignedTxn = ByteString.CopyFrom(retArr.ToArray());

            var result = await _client.SubmitTransactionAsync(
                 req, new Metadata());
            return result;
        }

        public void Shutdown()
        {
            Channel.ShutdownAsync().Wait();
        }
    }
}
