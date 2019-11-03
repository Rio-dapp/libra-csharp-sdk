using System;
using System.Linq;
using System.Collections.Generic;
using LibraAdmissionControlClient.LCS;
using LibraAdmissionControlClient.LCS.LCSTypes;

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

        public byte[] Blob { get { return _rawBytes; } }
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

            Console.WriteLine("Bytes - " + bytes.ByteArryToString());

            int startIndex = GetAssetTypeStartIndex();

            // .encode_struct(&self.authentication_key)?
            // .encode_u64(self.balance)?
            // .encode_bool(self.delegated_withdrawal_capability)?
            // .encode_u64(self.received_events_count)?
            // .encode_u64(self.sent_events_count)?
            // .encode_u64(self.sequence_number) ?;

            AssetType = LibraSettings.AssetType;
            startIndex += 9;
            AuthenticationKey = BitConverter.ToString(_rawBytes.SubArray(startIndex, 32)).Replace("-", "").ToLower();
            startIndex += 32;
            Balance = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));
            startIndex += 9;// 8+1 self.delegated_withdrawal_capability
            ReceivedEventsCount = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));
            startIndex += 8;
            SentEventsCount = BitConverter.ToUInt64(_rawBytes.SubArray(startIndex, 8));

            var bytesSeq = _rawBytes.Reverse().ToArray();
            //startIndex += 8;
            SequenceNumber = BitConverter.ToUInt64(
                bytesSeq.SubArray(0, 8).Reverse().ToArray());
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
