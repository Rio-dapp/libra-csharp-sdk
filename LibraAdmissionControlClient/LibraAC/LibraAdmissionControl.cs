using Grpc.Core;
using LibraAdmissionControlClient.Dtos;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Types;
using Google.Protobuf;

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
            Console.WriteLine("blob - " + blob.ByteArryToString());
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
                var info = transactions.Infos[i];

                CustomTransactionFullInfo ret = GetCustomTransactionFullInfo(transaction,
                    info);
                ret.Version = startVersion + (ulong)i;
                retList.Add(ret);
            }


            return retList;
        }

        private CustomTransactionFullInfo GetCustomTransactionFullInfo(SignedTransaction transaction,
            TransactionInfo info)
        {
            CustomTransactionFullInfo ret = new CustomTransactionFullInfo();

            if (transaction == null)
                return ret;

            var customRawTransaction = new CustomRawTransaction(
                transaction.RawTxnBytes.ToByteArray());
            ret.RawTransaction = customRawTransaction;
            ret.SenderPublicKey = transaction.SenderPublicKey.ToByteArray().ByteArryToString();
            ret.SenderSignature = transaction.SenderSignature.ToByteArray().ByteArryToString();
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
                transactions.Infos.FirstOrDefault());
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

            var trx = transaction.SignedTransaction;
            var customRawTransaction = new CustomRawTransaction(
                trx.RawTxnBytes.ToByteArray());
            return customRawTransaction;
        }

        public async Task<string> SendTransaction(
          byte[] privateKey, RawTransaction rawTransaction)
        {

            var result = await _service.SendTransactionAsync(privateKey, rawTransaction);
            return result.ToString();

        }

        public async Task<string> SendTransactionPtoP(
          byte[] senderPrivateKey, string sender, string reciver, ulong amount)
        {
            var senderAccount = await GetAccountInfoAsync(sender);

            RawTransaction rawTr = new RawTransaction()
            {
                ExpirationTime = (ulong)DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds(),
                GasUnitPrice = 0,
                MaxGasAmount = 29925,
                SequenceNumber = senderAccount.SequenceNumber
            };

            rawTr.Program = new Program();
            rawTr.Program.Code = ByteString.CopyFrom(Utility.PtPTrxBytecode);

            rawTr.Program.Arguments.Add(new TransactionArgument()
            {
                Type = TransactionArgument.Types.ArgType.Address,
                Data = ByteString.CopyFrom(reciver.HexStringToByteArray())
            });

            rawTr.Program.Arguments.Add(new TransactionArgument()
            {
                Type = TransactionArgument.Types.ArgType.U64,
                Data = ByteString.CopyFrom(BitConverter.GetBytes(amount))
            });

            rawTr.SenderAccount = ByteString.CopyFrom(sender.HexStringToByteArray());

            var result = await _service.SendTransactionAsync(senderPrivateKey, rawTr);
            return result.ToString();
        }

        public void Dispose()
        {
            _service.Shutdown();
        }
    }
}
