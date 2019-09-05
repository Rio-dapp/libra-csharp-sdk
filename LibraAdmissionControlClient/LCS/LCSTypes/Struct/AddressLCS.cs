using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public struct AddressLCS
    {
        public string Value { get; set; }
        public uint Length { get; internal set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
