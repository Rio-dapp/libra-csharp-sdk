using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Enum
{
    public enum ETransactionPayload
    {
        None = 0,
        Program = 3,
        WriteSet = 4,
        Script = 8,
        Module = 9,
    }
}
