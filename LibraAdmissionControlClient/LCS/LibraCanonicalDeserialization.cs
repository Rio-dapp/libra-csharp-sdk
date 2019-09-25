using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraAdmissionControlClient.LCS.LCSTypes;

namespace LibraAdmissionControlClient.LCS
{
    public class LibraCanonicalDeserialization
    {
        public byte[] U64ToByte(ulong source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] AddressToByte(AddressLCS source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Length);
            var data = source.Value.HexStringToByteArray();
            return len.Concat(data).ToArray();
        }

        public byte[] U32ToByte(uint source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] StringToByte(string source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Length);
            var data = Encoding.ASCII.GetBytes(source);
            return len.Concat(data).ToArray();
        }

        public byte[] ByteArrToByte(byte[] source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Length);
            var data = source;
            return len.Concat(data).ToArray();
        }

        public byte[] ListByteArrToByte(List<byte[]> source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Count);
            retArr = retArr.Concat(len).ToList();
            foreach (var item in source)
            {
                var localLen = U32ToByte((uint)item.Length);
                retArr = retArr.Concat(localLen).ToList();
                retArr = retArr.Concat(item).ToList();
            }
            return retArr.ToArray();
        }

        public byte[] BoolToByte(bool source)
        {
            return BitConverter.GetBytes(source);
        }

        public byte[] TransactionPayloadToByte(TransactionPayloadLCS source)
        {
            List<byte> retArr = new List<byte>();
            //var len = U32ToByte((uint)source);

            return retArr.ToArray();
        }

        public byte[] ProgramToByte(ProgramLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ScriptToByte(ScriptLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ModuleToByte(ModuleLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] TransactionArgumentToByte(TransactionArgumentLCS source)
        {
            throw new NotImplementedException();
        }

        public byte[] ListTransactionArgumentToByte(List<TransactionArgumentLCS> source)
        {
            throw new NotImplementedException();
        }
    }
}
