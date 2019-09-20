using LibraAdmissionControlClient;
using System.Linq;
using System;
using Types;
using Grpc.Core;
using static AdmissionControl.AdmissionControl;
using LibraAdmissionControlClient.Dtos;

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
            var address = "484fbead75efef9d9624df373106837d8188583a764b52269a3d4a8205eeafa1";
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
            address = "484fbead75efef9d9624df373106837d8188583a764b52269a3d4a8205eeafa1";
            var trx = service.GetTransactionsBySequenceNumberAsync(address, 0).Result;
            Console.WriteLine("Receiver = {0}, Amount = {1}", trx.Receiver, trx.Amount);

            #region TO DO with LCS
            ///---------------------
            /// SendTransaction PtP
            ///---------------------
            /// SendTransactionPtoP Use LibraAdmissionControlService method
            /// SendTransactionAsync(byte[] privateKey, RawTransaction rawTransaction)
            ///
            try
            {
                //Check account balance from the beginning
                var privateKey = new byte[] { 178, 112, 46, 189, 215, 71, 50, 157, 47, 25, 137, 23, 6, 213, 57, 174, 96, 12, 213, 116, 239, 162, 235, 187, 130, 34, 4, 136, 202, 221, 88, 170 };
                var publicKey = new byte[] { 37, 180, 53, 15, 179, 240, 136, 66, 29, 12, 170, 235, 203, 145, 158, 215, 28, 62, 134, 82, 213, 95, 78, 173, 39, 136, 29, 212, 132, 214, 199, 168 };
                string sender = "484fbead75efef9d9624df373106837d8188583a764b52269a3d4a8205eeafa1";
                //var result = service.SendTransactionPtoP(privateKey,
                //    sender,
                //    address,
                //    100).Result;
                //Console.WriteLine("SendTransaction Result = {0}", result);
                //  { "acStatus": { } } - Success
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
