using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient
{
    public static class LibraSettings
    {
        public const ushort ADDRESS_LENGTH  = 32;
        public static readonly string AssetType = "a208df134fefed8442b1f01fab59071898f5a1af5164e12c594de55a7004a91c";

        public static byte[] AssetTypeBytes { get{
                return AssetType.HexStringToByteArray();
            } }
    }
}
