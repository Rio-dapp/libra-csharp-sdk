using Grpc.Core;
using LibraAdmissionControlClient.Dtos;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Types;
using Google.Protobuf;
using LibraAdmissionControlClient.LCS.LCSTypes;
using LibraAdmissionControlClient.Enum;
using LibraAdmissionControlClient.LCS;

namespace LibraAdmissionControlClient
{
    public class LibraAdmissionControl : IDisposable
    {

        private string _host = "ac.testnet.libra.org";
        public string Host
        {
            get { return _host; }
        }

        private int _port = 8000;
        public int Port
        {
            get { return _port; }
        }

        public LibraAdmissionControlService AdmissionControlService { get { return _service; } }
        private LibraAdmissionControlService _service;
        public LibraAdmissionControl()
        {
            Initialize();
        }

        /// <summary>
        /// The Main method Initialize
        /// </summary>
        /// <param name="host">ac.testnet.libra.org</param>
        /// <param name="port">8000 or 30307</param>
        public LibraAdmissionControl(string host, int port)
        {
            Initialize(host, port);
        }

        public void Initialize(string host, int port)
        {
            _host = host;
            _port = port;
            Initialize();
        }

        public void Initialize()
        {
            _service = new LibraAdmissionControlService(_host, _port);
        }

        public async Task<CustomAccountResource> GetAccountInfoAsync(string address)
        {
            var account = await _service.GetAccountInfoAsync(address);
            var blob = account.Blob.Blob.ToByteArray();
            return new CustomAccountResource(blob); ;
        }

        public async Task<IEnumerable<CustomTransactionFullInfo>> GetTransactionsAsync(
            ulong startVersion, ulong limit)
        {
            var transactions = await _service.GetTransactionsAsync(startVersion, limit);

            List<CustomTransactionFullInfo> retList = new List<CustomTransactionFullInfo>();

            if (transactions == null)
                return retList;

            for (int i = 0; i < transactions.Transactions.Count; i++)
            {
                var transaction = transactions.Transactions[i];
                var info = transactions.Proof.TransactionInfos[i];

                CustomTransactionFullInfo ret = GetCustomTransactionFullInfo(transaction,
                  info);
                ret.Version = startVersion + (ulong)i;
                retList.Add(ret);
            }


            return retList;
        }

        private CustomTransactionFullInfo GetCustomTransactionFullInfo(Transaction transaction,
            TransactionInfo info)
        {
            CustomTransactionFullInfo ret = new CustomTransactionFullInfo();

            if (transaction == null)
                return ret;

            var customRawTransaction = new CustomRawTransaction(
                transaction.Transaction_.ToByteArray());
            ret.RawTransaction = customRawTransaction;
            // ret.SenderPublicKey = transaction..ToByteArray().ByteArryToString();
            // ret.SenderSignature = transaction.SenderSignature.ToByteArray().ByteArryToString();

            if (info != null)
                ret.GasUsed = info.GasUsed;
            else
                ret.GasUsed = 0;

            return ret;
        }

        public async Task<CustomTransactionFullInfo> GetTransactionAsync(
         ulong trxVersion)
        {
            var transactions = await _service.GetTransactionsAsync(trxVersion, 1);

            CustomTransactionFullInfo ret = GetCustomTransactionFullInfo(
                transactions.Transactions.FirstOrDefault(),
                transactions.Proof.TransactionInfos.FirstOrDefault());
            ret.Version = transactions.FirstTransactionVersion.Value;
            return ret;
        }



        public async Task<CustomRawTransaction> GetTransactionsBySequenceNumberAsync(
           string address, ulong sequenceNumber)
        {
            var transaction = await _service.GetTransactionsBySequenceNumberAsync(
                address, sequenceNumber);

            if (transaction == null)
                return null;

            var trx = transaction.Transaction;
            var customRawTransaction = new CustomRawTransaction(
                trx.Transaction_.ToByteArray());
            return customRawTransaction;
        }

        //public async Task<string> SendTransaction(
        //  byte[] privateKey, byte[] rawTransaction)
        //{

        //    var result = await _service.SendTransactionAsync(privateKey, rawTransaction);
        //    return result.ToString();

        //}

        public async Task<string> SendTransactionPtoP(
          byte[] senderPrivateKey, string sender, string reciver, ulong amount)
        {
            var senderAccount = await GetAccountInfoAsync(sender);

            RawTransactionLCS rawTr = new RawTransactionLCS()
            {
                ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60)
                .ToUnixTimeSeconds(),
                GasUnitPrice = 0,
                MaxGasAmount = 100000,
                SequenceNumber = senderAccount.SequenceNumber
            };

            rawTr.TransactionPayload = new TransactionPayloadLCS();

            rawTr.TransactionPayload.PayloadType = (uint)ETransactionPayloadLCS.Script;
            rawTr.TransactionPayload.Script = new ScriptLCS()
            {
                Code = Utility.PtPTrxBytecode,
                TransactionArguments = new List<TransactionArgumentLCS>() {
                     new TransactionArgumentLCS()
                     {
                         ArgType = (uint)ETransactionArgumentLCS.Address,
                         Address = new AddressLCS(reciver)
                     },
                     new TransactionArgumentLCS(){
                         ArgType = (uint)ETransactionArgumentLCS.U64,
                         U64 = amount
                     }
                }
            };
            rawTr.Sender = new AddressLCS(sender);
            var result = await _service.SendTransactionAsync(senderPrivateKey, rawTr);

            return result.ToString();
        }

        public async Task<string> SendTransactionModule(
        byte[] senderPrivateKey, string sender, byte[] module)
        {
            var senderAccount = await GetAccountInfoAsync(sender);

            RawTransactionLCS rawTr = new RawTransactionLCS()
            {
                ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60)
                .ToUnixTimeSeconds(),
                GasUnitPrice = 0,
                MaxGasAmount = 100000,
                SequenceNumber = senderAccount.SequenceNumber
            };

            rawTr.TransactionPayload = new TransactionPayloadLCS();
            rawTr.TransactionPayload.PayloadType = (uint)ETransactionPayloadLCS.Module;
            rawTr.TransactionPayload.Module = new ModuleLCS()
            {
                Code = module
            };
            rawTr.Sender = new AddressLCS(sender);
            var result = await _service.SendTransactionAsync(senderPrivateKey, rawTr);

            return result.ToString();
        }

        public void Dispose()
        {
            _service.Shutdown();
        }
    }
}
