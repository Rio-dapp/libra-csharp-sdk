using LibraAdmissionControlClient.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomProgram
    {
        public ETransactionPayload PayloadType { get; set; }
        public IEnumerable<byte[]> Modules { get; set; }
        public byte[] Code { get; set; }
        public IEnumerable<CustomTransactionArgument> Arguments { get; set; }
    }
}
