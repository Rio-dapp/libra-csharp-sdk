using System;
using System.Collections.Generic;
using System.Text;
using Types;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomRawTransaction
    {
        public ulong ExpirationTimeUnix { get; set; }
        public DateTime ExpirationTime { get; set; }
        public ulong GasUnitPrice { get; set; }
        public ulong MaxGasAmount { get; set; }
        public short PayloadCase { get; set; }
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
            RawTransaction rawTr = RawTransaction.Parser.ParseFrom(_rawTxnBytes);
            
            ExpirationTimeUnix = rawTr.ExpirationTime;
            ExpirationTime = rawTr.ExpirationTime.UnixTimeStampToDateTime();

            GasUnitPrice = rawTr.GasUnitPrice;
            MaxGasAmount = rawTr.MaxGasAmount;
            PayloadCase = (short)rawTr.PayloadCase;
            Sender = BitConverter.ToString(rawTr.SenderAccount.ToByteArray()).Replace("-", "").ToLower();
            SequenceNumber = rawTr.SequenceNumber;

            if (rawTr.Program != null)
            {
                Receiver = BitConverter.ToString(rawTr.Program.Arguments[0]
                    .Data.ToByteArray()).Replace("-", "").ToLower();
                Amount = BitConverter.ToUInt64(rawTr.Program
                    .Arguments[1].Data.ToByteArray());
                SequenceNumber = rawTr.SequenceNumber;

                List<CustomTransactionArgument> arguments = new List<CustomTransactionArgument>();
                foreach (var item in rawTr.Program.Arguments)
                {
                    var transactionArgument = new CustomTransactionArgument();
                    transactionArgument.Data = item.Data.ToByteArray();
                    transactionArgument.Type = (short)item.Type;
                    arguments.Add(transactionArgument);
                }

                List<byte[]> modules = new List<byte[]>();

                foreach (var item in rawTr.Program.Modules)
                    modules.Add(item.ToByteArray());

                Program = new CustomProgram()
                {
                    Arguments = arguments,
                    Code = rawTr.Program.Code.ToByteArray(),
                    Modules = modules
                };
            }
            else if (rawTr.Script != null)
            {

                Receiver = BitConverter.ToString(rawTr.Script.Arguments[0]
                  .Data.ToByteArray()).Replace("-", "").ToLower();
                Amount = BitConverter.ToUInt64(rawTr.Script.Arguments[1].Data.ToByteArray());
                SequenceNumber = rawTr.SequenceNumber;

                List<CustomTransactionArgument> arguments = new List<CustomTransactionArgument>();
                foreach (var item in rawTr.Script.Arguments)
                {
                    var transactionArgument = new CustomTransactionArgument();
                    transactionArgument.Data = item.Data.ToByteArray();
                    transactionArgument.Type = (short)item.Type;
                    arguments.Add(transactionArgument);
                }

                List<byte[]> modules = new List<byte[]>();

                Program = new CustomProgram()
                {
                    Arguments = arguments,
                    Code = rawTr.Script.Code.ToByteArray(),
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
