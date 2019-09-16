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
            var address = "6ad169626cd1ab2a220820c8fa9084be4190e65e80741929b000b80d8e8269fe";
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
            address = "1b3668e773996c75a2fb6ac6089c0c584c8f4c8f9050a7d8ae667aa317ffc15d";
            var trx = service.GetTransactionsBySequenceNumberAsync(address, 0).Result;
            Console.WriteLine("Receiver = {0}, Amount = {1}", trx.Receiver, trx.Amount);

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
                string sender = "9d410e33e1f4a07d9265362dce56e9b2a8b015d34c9723751d5f9afb7b6baee9";
                var result = service.SendTransactionPtoP(privateKey,
                    sender,
                    address,
                    100).Result;
                Console.WriteLine("SendTransaction Result = {0}", result);
                //  { "acStatus": { } } - Success
            }
            catch (Exception ex)
            {
                //Check account balance from the beginning
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
