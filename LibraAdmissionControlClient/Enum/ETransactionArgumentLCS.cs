using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Enum
{
    public enum ETransactionArgumentLCS
    {
        U64, // unsigned 64-bit integer
        Address, // Address represented as a variable length byte array
        String, // Variable length byte array of a string in UTF8 format
        ByteArray // Variable length byte array
    }
}
