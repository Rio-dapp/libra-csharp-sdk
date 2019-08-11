using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraAdmissionControlClient
{
    public static class Utility
    {
        public static readonly string MainAdress =
                    "0000000000000000000000000000000000000000000000000000000000000000";

        public static readonly byte[] PtPTrxBytecode = new byte[] { 76, 73, 66, 82, 65, 86, 77, 10, 1, 0, 7, 1, 74, 0, 0, 0, 4, 0, 0, 0, 3, 78, 0, 0, 0, 6, 0, 0, 0, 12, 84, 0, 0, 0, 6, 0, 0, 0, 13, 90, 0, 0, 0, 6, 0, 0, 0, 5, 96, 0, 0, 0, 41, 0, 0, 0, 4, 137, 0, 0, 0, 32, 0, 0, 0, 7, 169, 0, 0, 0, 14, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 1, 3, 0, 2, 0, 2, 4, 2, 0, 3, 0, 3, 2, 4, 2, 6, 60, 83, 69, 76, 70, 62, 12, 76, 105, 98, 114, 97, 65, 99, 99, 111, 117, 110, 116, 4, 109, 97, 105, 110, 15, 112, 97, 121, 95, 102, 114, 111, 109, 95, 115, 101, 110, 100, 101, 114, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 4, 0, 12, 0, 12, 1, 17, 1, 0, 2 };

        public static bool IsAddress(string adress)
        {
            if (string.IsNullOrEmpty(adress))
                return false;

            var arry = adress.HexStringToByteArray();
            if (arry.Length != 32)
                return false;

            return true;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static byte[] HexStringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static DateTime UnixTimeStampToDateTime(this ulong unixTimeStamp)
        {
            try
            {
                // TODO
                //1562008648525
                var dtDateTime = DateTimeOffset.FromUnixTimeSeconds((long)unixTimeStamp)
                                       .DateTime.ToLocalTime();
                return dtDateTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string ByteArryToString(this byte[] arr)
        {
            return BitConverter.ToString(arr).Replace("-", "").ToLower();
        }

        public static string ByteArryToString(this byte arr)
        {
            return BitConverter.ToString(new byte[] { arr })
                .Replace("-", "").ToLower();
        }

        public static string UIntToByteArry(this uint arr)
        {
            return BitConverter.ToString(
                   BitConverter.GetBytes(arr)).Replace("-", "").ToLower();
        }

        public static List<List<byte>> SplitToSublists(List<byte> source)
        {
            return source
                     .Select((x, i) => new { Index = i, Value = x })
                     .GroupBy(x => x.Index / 4)
                     .Select(x => x.Select(v => v.Value).ToList())
                     .ToList();
        }

        public static IEnumerable<byte> Read_u8(this IEnumerable<byte> source,
            ref int localCursor, int count)
        {
            var retArr = source.Skip(localCursor).Take(count).ToArray();
            localCursor += count;
            return retArr;
        }

        public static string ToShortAddress(this string address)
        {
            if (address == MainAdress)
                return "0x0";
            else
                return address;
        }
       
    }
}
