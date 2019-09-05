using LibraAdmissionControlClient.Enum;
using System;
using System.Collections.Generic;
using LibraAdmissionControlClient.LCS;
using LibraAdmissionControlClient.LCS.LCSTypes;
using System.Linq;
using Types;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomRawTransaction
    {
        public ulong ExpirationTimeUnix { get; set; }
        public DateTime ExpirationTime { get; set; }
        public ulong GasUnitPrice { get; set; }
        public ulong MaxGasAmount { get; set; }
        public ETransactionPayloadLCS PayloadCase { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public ulong Amount { get; set; }
        public ulong SequenceNumber { get; set; }
        public CustomProgram Program { get; set; }

        byte[] _rawTxnBytes;
        public CustomRawTransaction()
        {
        }

        public CustomRawTransaction(byte[] rawTxnBytes)
        {
            DeserializeRawTransaction(rawTxnBytes);
        }

        private void DeserializeRawTransaction(byte[] rawTxnBytes)
        {
            _rawTxnBytes = rawTxnBytes;
            int cursor = 0;
            RawTransactionLCS rawTr = _rawTxnBytes.LCSerialization<RawTransactionLCS>(ref cursor);

            ExpirationTimeUnix = rawTr.ExpirationTime;
            ExpirationTime = rawTr.ExpirationTime.UnixTimeStampToDateTime();

            GasUnitPrice = rawTr.GasUnitPrice;
            MaxGasAmount = rawTr.MaxGasAmount;
            PayloadCase = rawTr.TransactionPayload.PayloadTypeEnum;

            Sender = rawTr.Sender.Value;

            var programCode = rawTr.TransactionPayload.PayloadTypeEnum;
            //if (Utility.IsPtPOrMint(programCode))
            //    if (rawTr.TransactionPayload.PayloadTypeEnum == ETransactionPayloadLCS.Program)
            //{
                var args = rawTr.TransactionPayload.Program.TransactionArguments.ToArray();
            if (args[0].ArgTypeEnum == ETransactionArgumentLCS.Address &&
                args[0].ArgTypeEnum == ETransactionArgumentLCS.U64)
            {
                Receiver = args[0].Address.Value;
                Amount = args[1].U64;
            }
            //}
            SequenceNumber = rawTr.SequenceNumber;

            if (rawTr.TransactionPayload.PayloadTypeEnum == ETransactionPayloadLCS.Program)
            {
                List<CustomTransactionArgument> arguments = new List<CustomTransactionArgument>();
                foreach (var item in rawTr.TransactionPayload.Program.TransactionArguments)
                {
                    var transactionArgument = new CustomTransactionArgument();

                    //if (true)
                    //{
                    //    transactionArgument.Data = item.Address.Value;
                    //    transactionArgument.Type = (short)item.Type;
                    //}
                    

                    arguments.Add(transactionArgument);
                }

                List<byte[]> modules = new List<byte[]>();

                foreach (var item in rawTr.TransactionPayload.Program.Modules)
                    modules.Add(item);

                Program = new CustomProgram()
                {
                    Arguments = arguments,
                    Code = rawTr.TransactionPayload.Program.CodeString,
                    Modules = modules
                };
            }
        }

        public override string ToString()
        {
            return "{\n   ExpirationTime : " + ExpirationTime + "\n" +
                    "   GasUnitPrice : " + GasUnitPrice + "\n" +
                    "   Sender : " + Sender + "\n" +
                    "   Receiver : " + Receiver + "\n" +
                    "   Amount : " + Amount + "\n" +
                    "   SequenceNumber : " + SequenceNumber + "\n}";
        }


    }
}
