using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS
{
    public class LibraCanonicalDeserialization
    {
        public byte[] U64ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

    }
}
