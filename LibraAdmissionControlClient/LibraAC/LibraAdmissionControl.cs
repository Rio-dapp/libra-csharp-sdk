using Grpc.Core;
using LibraAdmissionControlClient.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<CustomRawTransaction>> GetTransactionsAsync(
            ulong startVersion, ulong limit)
        {
            var transactions = await _service.GetTransactionsAsync(startVersion, limit);

            List<CustomRawTransaction> retList = new List<CustomRawTransaction>();

            if (transactions == null)
                return retList;

            foreach (var transaction in transactions.Transactions)
            {
                var customRawTransaction = new CustomRawTransaction(
                    transaction.RawTxnBytes.ToByteArray());
                retList.Add(customRawTransaction);
            }
            return retList;
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

        public void Dispose()
        {
            _service.Shutdown();
        }
    }
}
