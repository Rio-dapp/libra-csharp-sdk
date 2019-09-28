using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public struct AddressLCS
    {
        public AddressLCS(string source)
        {
            this.Value = source;
            this.ValueByte = source.HexStringToByteArray();
            this.Length = (uint)this.ValueByte.Length;
        }

        public byte[] ValueByte { get; set; }
        public string Value { get; set; }
        public uint Length { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
