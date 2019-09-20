using LibraAdmissionControlClient.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public class WriteOpLCS
    {
        public uint WriteOpType { get; internal set; }
        public EWriteOpLCS WriteOpTypeEnum
        {
            get
            {
                return (EWriteOpLCS)WriteOpType;
            }
        }

        public byte[] Value { get; internal set; }
        public override string ToString()
        {
            if (WriteOpTypeEnum == EWriteOpLCS.Value)
            {
                return Value.ByteArryToString();
            }
            else
                return "Deletion";
        }
    }
}
