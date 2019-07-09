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
            var address = "881852936f0d63b3bb53463355df27a5b89a76cc8a09e269062c76dbb0f58e6a";
            var account = service.GetAccountInfoAsync(address).Result;
            Console.WriteLine(account.Balance);

            ///---------------------
            /// GetTransactions
            ///---------------------
            var start = 1000u;
            var limit = 20u;
            var transactions = service.GetTransactionsAsync(start, limit).Result;
            Console.WriteLine(transactions.Count());

            ///---------------------
            /// GetTransactions by Seqenc number
            ///---------------------
            address = "881852936f0d63b3bb53463355df27a5b89a76cc8a09e269062c76dbb0f58e6a";
            var trx = service.GetTransactionsBySequenceNumberAsync(address, 0).Result;
            Console.WriteLine("Receiver = {0}, Amount = {1}", trx.Receiver, trx.Amount);

         

            Console.ReadKey();
        }
    }
}
