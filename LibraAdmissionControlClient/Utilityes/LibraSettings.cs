using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient
{
    public static class LibraSettings
    {
        public const ushort ADDRESS_LENGTH  = 32;
        public static readonly string AssetType = "217da6c6b3e19f1825cfb2676daecce3bf3de03cf26647c78df00b371b25cc97";

        public static byte[] AssetTypeBytes { get{
                return AssetType.HexStringToByteArray();
            } }
    }
}
