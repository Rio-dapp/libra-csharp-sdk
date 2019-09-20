using LibraAdmissionControlClient.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public class TransactionArgumentLCS
    {
        public uint ArgType { get; internal set; }
        public ETransactionArgumentLCS ArgTypeEnum
        {
            get
            {
                return (ETransactionArgumentLCS)ArgType;
            }
        }

        public ulong U64 { get; internal set; }
        public AddressLCS Address { get; internal set; }
        public byte[] ByteArray { get; internal set; }
        public string String { get; internal set; }

        public override string ToString()
        {
            if (ArgTypeEnum == ETransactionArgumentLCS.Address)
                return $"[{ArgTypeEnum} , {Address}]";
            else if (ArgTypeEnum == ETransactionArgumentLCS.ByteArray)
                return $"[{ArgTypeEnum} , {ByteArray.ByteArryToString()}]";
            // return $"[{ArgTypeEnum} / { Encoding.UTF8.GetString(ByteArray)}]";
            else if (ArgTypeEnum == ETransactionArgumentLCS.String)
                return $"[{ArgTypeEnum} , {String}]";
            else if (ArgTypeEnum == ETransactionArgumentLCS.U64)
                return $"[{ArgTypeEnum} , {U64}]";

            return "TransactionArgumentLCS is null";
        }
    }
}
