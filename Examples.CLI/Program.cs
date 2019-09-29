using LibraAdmissionControlClient;
using System.Linq;
using System;
using Types;
using Grpc.Core;
using static AdmissionControl.AdmissionControl;
using LibraAdmissionControlClient.Dtos;
using LibraAdmissionControlClient.LCS.LCSTypes;
using LibraAdmissionControlClient.LCS;
using NSec.Cryptography;
using System.Text;

namespace Examples.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            LibraAdmissionControl service = new LibraAdmissionControl();
            ///---------------------
            /// GetAccountInfo
            ///---------------------
            var address = "500fd2c4e7e8e031df9b76af89a06af97d32abf80689c92b45c4e987bacdcecf";
            var account = service.GetAccountInfoAsync(address).Result;
            Console.WriteLine(account.Balance);
            Console.WriteLine(account.SequenceNumber);

            ///---------------------
            /// GetTransactions
            ///---------------------
            var start = 10068u;
            var limit = 10u;
            var transactions = service.GetTransactionsAsync(start, limit).Result;
            Console.WriteLine(transactions.Count());

            ///---------------------
            /// GetTransactions by Seqenc number
            ///---------------------
            address = "5d2e159c1ac8ad0c4ac2071b4a977bf0103a4ae469e186093aa5e03efc1e0afe";
            var trx = service.GetTransactionsBySequenceNumberAsync(address, 0).Result;
            Console.WriteLine("Receiver = {0}, Amount = {1}", trx.Receiver, trx.Amount);

            #region LCS test
            AddressLCS addressLcs = new AddressLCS(address);
            byte[] addressByteLcs = LCSCore.LCSerialize(addressLcs);
            addressLcs = LCSCore.LCDeserialize<AddressLCS>(addressByteLcs);
            Console.WriteLine("LCS - " + addressLcs);
            #endregion

            #region SendTransaction
            ///---------------------
            /// SendTransaction PtoP
            ///---------------------
            try
            {
                //Check account balance from the beginning
                var privateKey = new byte[] { 82, 86, 29, 56, 85, 21, 64, 101, 182, 161, 68, 237, 96, 47, 86, 108, 60, 106, 231, 218, 202, 31, 215, 3, 131, 208, 224, 94, 96, 89, 149, 168 };
                string sender = "36dba2da4eb4ee1f9955800940a029d435a6bc1cd4ad748ec63a9d9e4410c345";
                var result = service.SendTransactionPtoP(
                    privateKey,
                    sender,
                    address,
                    100).Result;
                Console.WriteLine("SendTransaction Result = {0}", result);
                //{ "acStatus": { } }
                // - Success
            }
            catch (Exception ex)
            {
                //Check account balance from the beginning
                Console.WriteLine(ex.Message);
            }
            #endregion



            Console.ReadKey();
        }
    }
}
