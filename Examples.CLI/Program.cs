using LibraAdmissionControlClient;
using System.Linq;
using System;
using LibraAdmissionControlClient.LCS;
using Types;
using Grpc.Core;
using static AdmissionControl.AdmissionControl;
using LibraAdmissionControlClient.Dtos;
using LibraAdmissionControlClient.LCS.LCSTypes;

namespace Examples.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
           

            ///---------------------
            /// LCS example with program
            ///---------------------
            byte[] trxWithProgram = "200000003A24A61E05D129CACE9E0EFC8BC9E33831FEC9A9BE66F50FD352A2638A49B9EE200000000000000000000000040000006D6F766502000000020000000900000043414645204430304402000000090000006361666520643030640300000001000000CA02000000FED0010000000D1027000000000000204E0000000000008051010000000000"
                 .ToLower().HexStringToByteArray();
            int corsor = 0;
            RawTransactionLCS rawTransactionLCS
                = trxWithProgram.LCSerialization<RawTransactionLCS>(ref corsor);
            Console.WriteLine("with program = " + rawTransactionLCS);

            Console.WriteLine("----------------------------------------------");

            ///---------------------
            /// LCS example write set
            ///---------------------
            byte[] trxWithWriteSet = "20000000C3398A599A6F3B9F30B635AF29F2BA046D3A752C26E9D0647B9647D1F4C04AD42000000000000000010000000200000020000000A71D76FAA2D2D5C3224EC3D41DEB293973564A791E55C6782BA76C2BF0495F9A2100000001217DA6C6B3E19F1825CFB2676DAECCE3BF3DE03CF26647C78DF00B371B25CC970000000020000000C4C63F80C74B11263E421EBF8486A4E398D0DBC09FA7D4F62CCDB309F3AEA81F0900000001217DA6C6B3E19F180100000004000000CAFED00D00000000000000000000000000000000FFFFFFFFFFFFFFFF"
                  .ToLower().HexStringToByteArray();
            int corsor2 = 0;
            RawTransactionLCS rawTransactionLCS2
                = trxWithWriteSet.LCSerialization<RawTransactionLCS>(ref corsor2);
            Console.WriteLine("with write set = " + rawTransactionLCS2);

            #region TO DO with LCS

            //LibraAdmissionControl service = new LibraAdmissionControl();
            /////---------------------
            ///// GetAccountInfo
            /////---------------------
            //var address = "9d410e33e1f4a07d9265362dce56e9b2a8b015d34c9723751d5f9afb7b6baee9";
            //var account = service.GetAccountInfoAsync(address).Result;
            //Console.WriteLine(account.Balance);
            //Console.WriteLine(account.SequenceNumber);

            /////---------------------
            ///// GetTransactions
            /////---------------------
            //var start = 1000u;
            //var limit = 20u;
            //var transactions = service.GetTransactionsAsync(start, limit).Result;
            //Console.WriteLine(transactions.Count());

            /////---------------------
            ///// GetTransactions by Seqenc number
            /////---------------------
            //address = "9d410e33e1f4a07d9265362dce56e9b2a8b015d34c9723751d5f9afb7b6baee9";
            //var trx = service.GetTransactionsBySequenceNumberAsync(address, 0).Result;
            //Console.WriteLine("Receiver = {0}, Amount = {1}", trx.Receiver, trx.Amount);

            ///---------------------
            /// SendTransaction PtP
            ///---------------------
            /// SendTransactionPtoP Use LibraAdmissionControlService method
            /// SendTransactionAsync(byte[] privateKey, RawTransaction rawTransaction)
            /// 
            //var privateKey = new byte[] { 178, 112, 46, 189, 215, 71, 50, 157, 47, 25, 137, 23, 6, 213, 57, 174, 96, 12, 213, 116, 239, 162, 235, 187, 130, 34, 4, 136, 202, 221, 88, 170 };
            //var publicKey = new byte[] { 37, 180, 53, 15, 179, 240, 136, 66, 29, 12, 170, 235, 203, 145, 158, 215, 28, 62, 134, 82, 213, 95, 78, 173, 39, 136, 29, 212, 132, 214, 199, 168 };
            //string sender = "9d410e33e1f4a07d9265362dce56e9b2a8b015d34c9723751d5f9afb7b6baee9";
            //var result = service.SendTransactionPtoP(privateKey, 
            //    sender, 
            //    address, 
            //    100).Result;
            //Console.WriteLine("SendTransaction Result = {0}", result);
            //  { "acStatus": { } } - Success 
            #endregion

            Console.ReadKey();
        }
    }
}
