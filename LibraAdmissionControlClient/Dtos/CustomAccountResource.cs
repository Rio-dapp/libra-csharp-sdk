using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.Dtos
{
    public class CustomAccountResource
    {
        public ulong Balance { get; set; }
        public ulong SequenceNumber { get; set; }

        /// <summary>
        /// Addreass
        /// </summary>
        public string AuthenticationKey { get; set; }

        public string AssetType { get; set; }
        public ulong SentEventsCount { get; set; }
        public ulong ReceivedEventsCount { get; set; }

        byte[] _rawBytes;

        public CustomAccountResource()
        {

        }
        public CustomAccountResource(byte[] bytes)
        {
            DeserializeBlob(bytes);
        }

        /// <summary>
        /// Blob to AccountResource
        /// </summary>
        /// <param name="bytes">UTF8</param>
        public void DeserializeBlob(byte[] bytes)
        {
            _rawBytes = bytes;

            int startIndex = GetAssetTypeStartIndex();

            //AssetType = BitConverter.ToString(_rawBytes.SubArray(9, 32)).Replace("-", "");//9, 40
            AssetType = LibraSettings.AssetType;
            startIndex += 9;
            AuthenticationKey = BitConverter.ToString(_rawBytes.SubArray(startIndex, 32)).Replace("-", "").ToLower();//SubArray(49, 32) 49, 80
            startIndex += 32;
            Balance = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));//SubArray(81, 8) 81, 88
            startIndex += 8;
            ReceivedEventsCount = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));//SubArray(89, 8) 89, 96
            startIndex += 8;
            SentEventsCount = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));//SubArray(97, 8) 97, 104
            startIndex += 8;
            SequenceNumber = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));//SubArray(105, 8) 105, 112
        }

        private int GetAssetTypeStartIndex()
        {
            List<byte> assetTypeBytes = new List<byte>();
            int startIndex = 0;

            for (int i = 0; i < _rawBytes.Length; i++)
            {
                var item = _rawBytes[i];

                if (assetTypeBytes.Count == 32)
                    assetTypeBytes.Remove(assetTypeBytes.FirstOrDefault());
                assetTypeBytes.Add(item);

                if (assetTypeBytes.SequenceEqual(LibraSettings.AssetTypeBytes))
                {
                    startIndex = i;
                    continue;
                }
            }

            return startIndex;
        }

        public override string ToString()
        {
            return "{\n   AssetType : " + AssetType + "\n" +
                    "   AuthenticationKey : " + AuthenticationKey + "\n" +
                    "   Balance : " + Balance + "\n" +
                    "   ReceivedEventsCount : " + ReceivedEventsCount + "\n" +
                    "   SequenceNumber : " + SequenceNumber + "\n" +
                    "   SentEventsCount : " + SentEventsCount + "\n}";
        }
    }
}
