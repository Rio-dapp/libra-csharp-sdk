using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomTransactionArgument
    {
        public byte[] Data { get; set; }
        public short Type { get; set; }
    }
}
