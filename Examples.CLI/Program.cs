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
            /// GetTransactions
            ///---------------------
            var start = 0u;
            var limit = 10u;
            var transactions = service.GetTransactionsAsync(start, limit).Result;
            Console.WriteLine(transactions.Count());

            ///---------------------
            /// GetAccountInfo
            ///---------------------
            var address = "03d7cb76e3429ca23fc16ee1bc323ae928400a5040fb8993d2eeabc4b361f42b";
            var account = service.GetAccountInfoAsync(address).Result;
            Console.WriteLine(account.Balance);
            Console.WriteLine(account.SequenceNumber);

            #region LCS test
            AddressLCS addressLcs = new AddressLCS(address);
            byte[] addressByteLcs = LCSCore.LCSerialize(addressLcs);
            addressLcs = LCSCore.LCDeserialize<AddressLCS>(addressByteLcs);
            Console.WriteLine("LCS - " + addressLcs);
            #endregion

            #region SendTransaction
            ///---------------------
            /// SendTransaction PtP
            ///---------------------
            //Check account balance from the beginning
            var privateKey = new byte[] { 82, 86, 29, 56, 85, 21, 64, 101, 182, 161, 68, 237, 96, 47, 86, 108, 60, 106, 231, 218, 202, 31, 215, 3, 131, 208, 224, 94, 96, 89, 149, 168 };
            string sender = "36dba2da4eb4ee1f9955800940a029d435a6bc1cd4ad748ec63a9d9e4410c345";
            try
            {
                var result = service.SendTransactionPtoP(
                    privateKey,
                    sender,
                    address,
                    10).Result;
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

            #region Publish Module
            var module = new byte[] { 76, 73, 66, 82, 65, 86, 77, 10, 1, 0, 11, 1, 110, 0, 0, 0, 2, 0, 0, 0, 2, 112, 0, 0, 0, 4, 0, 0, 0, 3, 116, 0, 0, 0, 18, 0, 0, 0, 12, 134, 0, 0, 0, 4, 0, 0, 0, 13, 138, 0, 0, 0, 42, 0, 0, 0, 14, 180, 0, 0, 0, 48, 0, 0, 0, 5, 228, 0, 0, 0, 42, 0, 0, 0, 4, 14, 1, 0, 0, 32, 0, 0, 0, 9, 46, 1, 0, 0, 4, 0, 0, 0, 10, 50, 1, 0, 0, 6, 0, 0, 0, 11, 56, 1, 0, 0, 118, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 3, 1, 0, 4, 2, 0, 5, 1, 0, 6, 3, 0, 7, 4, 1, 2, 1, 1, 2, 1, 7, 0, 0, 2, 2, 1, 0, 2, 0, 1, 6, 7, 0, 0, 0, 2, 0, 2, 6, 7, 0, 0, 2, 0, 2, 0, 2, 6, 7, 0, 0, 1, 0, 2, 0, 1, 7, 0, 0, 0, 3, 2, 2, 1, 3, 0, 3, 2, 6, 7, 0, 0, 6, 2, 3, 3, 6, 7, 0, 0, 2, 6, 2, 3, 3, 6, 7, 0, 0, 6, 1, 1, 3, 3, 6, 7, 0, 0, 1, 6, 1, 3, 3, 7, 0, 0, 2, 1, 5, 82, 84, 101, 115, 116, 1, 84, 3, 110, 101, 119, 2, 116, 49, 2, 116, 50, 2, 116, 51, 2, 116, 52, 9, 100, 101, 115, 116, 114, 111, 121, 95, 116, 4, 102, 105, 110, 116, 2, 102, 114, 12, 3, 123, 235, 224, 10, 235, 48, 138, 129, 204, 244, 105, 168, 125, 242, 195, 86, 179, 51, 244, 158, 68, 156, 44, 219, 91, 44, 253, 143, 13, 146, 0, 2, 2, 0, 0, 8, 0, 0, 9, 1, 0, 1, 0, 2, 0, 4, 0, 12, 0, 12, 1, 20, 0, 1, 2, 1, 1, 0, 2, 2, 7, 0, 12, 0, 16, 0, 13, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 12, 1, 23, 2, 2, 1, 0, 2, 3, 7, 0, 12, 0, 16, 0, 13, 2, 12, 1, 12, 2, 23, 2, 3, 1, 0, 2, 4, 9, 0, 12, 0, 16, 1, 13, 1, 9, 13, 2, 12, 2, 12, 1, 23, 2, 4, 1, 0, 2, 5, 7, 0, 12, 0, 16, 1, 13, 2, 12, 1, 12, 2, 23, 2, 5, 1, 0, 1, 6, 5, 0, 12, 0, 21, 0, 1, 13, 2, 13, 1, 2 };
            var resultM = service.SendTransactionModule(
                  privateKey,
                  sender, module
                ).Result;
            Console.WriteLine("Publish Module Result = {0}", resultM);
            #endregion

            try
            {
                ///---------------------
                /// GetTransactions by Seqenc number
                ///---------------------
                address = "5d2e159c1ac8ad0c4ac2071b4a977bf0103a4ae469e186093aa5e03efc1e0afe";
                var trx = service.GetTransactionsBySequenceNumberAsync(address, 0).Result;
                Console.WriteLine("Receiver = {0}, Amount = {1}", trx.Receiver, trx.Amount);
            }
            catch
            {
            }

            Console.ReadKey();
        }
    }
}
