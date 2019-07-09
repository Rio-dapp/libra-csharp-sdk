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

        public static bool IsAddress(string adress)
        {
            if (string.IsNullOrEmpty(adress))
                return false;

            var arry = adress.StringToByteArray();
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

        public static byte[] StringToByteArray(this string hex)
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
