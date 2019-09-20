using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public class AccountEventLCS
    {
        public string Account { get; internal set; }
        public ulong Amount { get; internal set; }
        public override string ToString()
        {
            return "{ Account = " + Account + "," + Environment.NewLine +
                "Amount = " + Amount
                + "}";
        }
    }
}
