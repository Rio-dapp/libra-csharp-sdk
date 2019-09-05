using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS
{
    /// <summary>
    /// TO DO
    /// </summary>
    public class LibraCanonicalDeserialization
    {
        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public byte[] U64ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

    }
}
