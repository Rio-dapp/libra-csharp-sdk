using System;
using System.Collections.Generic;
using System.Text;
using Types;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomTransactionFullInfo
    {
        public CustomRawTransaction RawTransaction { get; set; }
        public ulong Version { get; internal set; }
        public string SenderPublicKey { get; internal set; }
        public string SenderSignature { get; internal set; }
        public ulong GasUsed { get; internal set; }
    }
}
