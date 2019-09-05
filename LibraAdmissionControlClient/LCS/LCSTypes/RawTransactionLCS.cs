using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public class RawTransactionLCS
    {
        public ulong MaxGasAmount { get; internal set; }
        public ulong GasUnitPrice { get; internal set; }
        public ulong ExpirationTime { get; internal set; }
        public AddressLCS Sender { get; internal set; }
        public ulong SequenceNumber { get; internal set; }
        public TransactionPayloadLCS TransactionPayload { get; internal set; }

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
