using LibraAdmissionControlClient.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomTransactionArgument
    {
        public ETransactionArgumentLCS ArgTypeEnum
        {
            get
            {
                return (ETransactionArgumentLCS)ArgType;
            }
        }
        public uint ArgType { get; internal set; }
        public ulong U64 { get; internal set; }
        public string Address { get; internal set; }
        public byte[] ByteArray { get; internal set; }
        public string String { get; internal set; }
    }
}
