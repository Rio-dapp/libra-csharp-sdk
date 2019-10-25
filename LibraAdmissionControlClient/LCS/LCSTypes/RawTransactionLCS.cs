using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public class RawTransactionLCS
    {
        public ulong MaxGasAmount { get;  set; }
        public ulong GasUnitPrice { get;  set; }
        public ulong ExpirationTime { get;  set; }
        public AddressLCS Sender { get;  set; }
        public ulong SequenceNumber { get;  set; }
        public TransactionPayloadLCS TransactionPayload { get;  set; }
        public uint FirstUint { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" +
                string.Format("Sender = {0},{1}", Sender, Environment.NewLine);
            retStr +=
                string.Format("SequenceNumber = {0},{1}", SequenceNumber, Environment.NewLine);
            retStr +=
                string.Format("TransactionPayload = {0},{1}", TransactionPayload,
                     Environment.NewLine);
            retStr +=
                string.Format("MaxGasAmount = {0},{1}", MaxGasAmount, Environment.NewLine);
            retStr +=
               string.Format("GasUnitPrice = {0},{1}", GasUnitPrice, Environment.NewLine);
            retStr +=
               string.Format("ExpirationTime = {0}", ExpirationTime) +
               "}";
            return retStr;
        }
    }
}
