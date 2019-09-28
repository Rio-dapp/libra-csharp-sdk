using LibraAdmissionControlClient.LCS.LCSTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraAdmissionControlClient.LCS
{
    public class LibraCanonicalSerialization
    {
        const int ADDRESS_LENGTH = 32;

        public AccountResourceLCS GetAccountResource(byte[] source, ref int cursor)
        {
            var retVal = new AccountResourceLCS();

            retVal.AuthenticationKey = source.LCSerialize<AddressLCS>(ref cursor);
            retVal.Balance = Read_u64(source, ref cursor);
            retVal.DelegatedWithdrawalCapability = source.LCSerialize<bool>(ref cursor);
            retVal.ReceivedEvents = source.LCSerialize<byte[]>(ref cursor);
            retVal.SentEvents = source.LCSerialize<byte[]>(ref cursor);
            retVal.SequenceNumber = Read_u64(source, ref cursor);

            return retVal;
        }

        public RawTransactionLCS GetRawTransaction(byte[] source, ref int cursor)
        {
            var retVal = new RawTransactionLCS();

            retVal.Sender = source.LCSerialize<AddressLCS>(ref cursor);
            retVal.SequenceNumber = source.LCSerialize<ulong>(ref cursor);
            retVal.TransactionPayload =
                source.LCSerialize<TransactionPayloadLCS>(ref cursor);
            retVal.MaxGasAmount = Read_u64(source, ref cursor);
            retVal.GasUnitPrice = Read_u64(source, ref cursor);
            retVal.ExpirationTime = Read_u64(source, ref cursor);

            return retVal;
        }

        public WriteOpLCS GetWriteOp(byte[] source, ref int cursor)
        {
            var retVal = new WriteOpLCS();
            retVal.WriteOpType = source.LCSerialize<uint>(ref cursor);

            if (retVal.WriteOpTypeEnum == Enum.EWriteOpLCS.Value)
                retVal.Value = source.LCSerialize<byte[]>(ref cursor);

            return retVal;
        }

        public AccessPathLCS GetAccessPath(byte[] source, ref int cursor)
        {
            var retVal = new AccessPathLCS();

            retVal.Address = source.LCSerialize<AddressLCS>(ref cursor);
            retVal.Path = source.LCSerialize<byte[]>(ref cursor);

            return retVal;
        }

        public AccountEventLCS GetAccountEvent(byte[] source, ref int cursor)
        {
            var retVal = new AccountEventLCS();

            retVal.Amount = source.LCSerialize<ulong>(ref cursor);
            retVal.Account = source.LCSerialize<byte[]>(ref cursor)
                .ByteArryToString();

            return retVal;
        }

        public WriteSetLCS GetWriteSet(byte[] source, ref int cursor)
        {
            var retVal = new WriteSetLCS();
            retVal.WriteSet = new Dictionary<AccessPathLCS, WriteOpLCS>();
            retVal.Length = Read_u32(source, ref cursor);

            for (int i = 0; i < retVal.Length; i++)
            {
                var key = source.LCSerialize<AccessPathLCS>(ref cursor);
                var value = source.LCSerialize<WriteOpLCS>(ref cursor);

                retVal.WriteSet.Add(key, value);
            }
            return retVal;
        }

        public bool GetBool(byte[] source, ref int cursor)
        {
            return Convert.ToBoolean(Read_u8(source, ref cursor, 1).FirstOrDefault());
        }

        public string GetString(byte[] source, ref int cursor)
        {
            var length = Read_u32(source, ref cursor);
            return Read_String(source, ref cursor, (int)length);
        }

        public IEnumerable<byte[]> GetListByteArrays(byte[] source, ref int cursor)
        {
            List<byte[]> retVal = new List<byte[]>();
            var length = Read_u32(source, ref cursor);

            for (int i = 0; i < length; i++)
                retVal.Add(source.LCSerialize<byte[]>(ref cursor));

            return retVal;
        }

        public byte[] GetByteArray(byte[] source, ref int cursor)
        {
            var length = Read_u32(source, ref cursor);

            var retVal = Read_u8(source, ref cursor, (int)length)
                .ToArray();

            return retVal;
        }

        public byte GetByte(byte[] source, ref int cursor)
        {
            return Read_u8(source, ref cursor, 1).FirstOrDefault();
        }

        public AddressLCS GetAddress(byte[] source, ref int cursor)
        {
            var retVal = new AddressLCS();
            retVal.Length = Read_u32(source, ref cursor);

            retVal.ValueByte = Read_u8(source, ref cursor, (int)retVal.Length)
                .ToArray();
            retVal.Value = retVal.ValueByte
                .ByteArryToString();

            return retVal;
        }

        public ulong GetU64(byte[] source, ref int cursor)
        {
            return Read_u64(source, ref cursor);
        }

        public uint GetU32(byte[] source, ref int cursor)
        {
            return Read_u32(source, ref cursor);
        }

        public TransactionPayloadLCS GetTransactionPayload(byte[] source, ref int cursor)
        {
            var retVal = new TransactionPayloadLCS();
            retVal.PayloadType = Read_u32(source, ref cursor);

            if (retVal.PayloadTypeEnum == Enum.ETransactionPayloadLCS.Program)
                retVal.Program = source.LCSerialize<ProgramLCS>(ref cursor);
            else if (retVal.PayloadTypeEnum == Enum.ETransactionPayloadLCS.WriteSet)
                retVal.WriteSet = source.LCSerialize<WriteSetLCS>(ref cursor);
            else if (retVal.PayloadTypeEnum == Enum.ETransactionPayloadLCS.Script)
                retVal.Script = source.LCSerialize<ScriptLCS>(ref cursor);
            else if (retVal.PayloadTypeEnum == Enum.ETransactionPayloadLCS.Module)
                retVal.Module = source.LCSerialize<ModuleLCS>(ref cursor);

            return retVal;
        }

        public ProgramLCS GetProgram(byte[] source, ref int cursor)
        {
            var retVal = new ProgramLCS();
            //struct Program
            // {
            //  code: Vec<u8>, // Variable length byte array
            //  args: Vec<TransactionArgument>, // Variable length array of TransactionArguments
            //  modules: Vec<Vec<u8>>, // Variable length array of variable length byte arrays
            // }
            retVal.Code = source.LCSerialize<byte[]>(ref cursor);
            retVal.TransactionArguments =
                source.LCSerialize<List<TransactionArgumentLCS>>(ref cursor);
            retVal.Modules = source.LCSerialize<List<byte[]>>(ref cursor);

            return retVal;
        }
        public ScriptLCS GetScript(byte[] source, ref int cursor)
        {
            var retVal = new ScriptLCS();
            retVal.Code = source.LCSerialize<byte[]>(ref cursor);
            retVal.TransactionArguments =
                source.LCSerialize<List<TransactionArgumentLCS>>(ref cursor);

            return retVal;
        }
        public ModuleLCS GetModule(byte[] source, ref int cursor)
        {
            var retVal = new ModuleLCS();
            retVal.Code = source.LCSerialize<byte[]>(ref cursor);
            return retVal;
        }

        public TransactionArgumentLCS GetTransactionArgument(byte[] source,
            ref int cursor)
        {
            var retVal = new TransactionArgumentLCS();
            retVal.ArgType = Read_u32(source, ref cursor);

            if (retVal.ArgTypeEnum == Enum.ETransactionArgumentLCS.U64)
            {
                retVal.U64 = Read_u64(source, ref cursor);
            }
            else if (retVal.ArgTypeEnum == Enum.ETransactionArgumentLCS.Address)
            {
                retVal.Address = source.LCSerialize<AddressLCS>(ref cursor);
            }
            else if (retVal.ArgTypeEnum == Enum.ETransactionArgumentLCS.ByteArray)
            {
                retVal.ByteArray = source.LCSerialize<byte[]>(ref cursor);
            }
            else if (retVal.ArgTypeEnum == Enum.ETransactionArgumentLCS.String)
            {
                retVal.String = source.LCSerialize<string>(ref cursor);
            }

            return retVal;
        }

        public IEnumerable<TransactionArgumentLCS> GetTransactionArguments(
            byte[] source, ref int cursor)
        {
            var retVal = new List<TransactionArgumentLCS>();
            var length = Read_u32(source, ref cursor);

            for (int i = 0; i < length; i++)
                retVal.Add(source.LCSerialize<TransactionArgumentLCS>(ref cursor));

            return retVal;
        }
        #region Helpers

        public IEnumerable<byte> Read_u8(IEnumerable<byte> source,
          ref int localCursor, int count)
        {
            var retArr = source.Skip(localCursor).Take(count);
            localCursor += count;
            return retArr;
        }

        public string Read_String(IEnumerable<byte> source,
         ref int localCursor, int count)
        {
            var retArr = Read_u8(source, ref localCursor, count);
            return Encoding.UTF8.GetString(retArr.ToArray());
        }

        public ushort Read_u16(IEnumerable<byte> source,
          ref int localCursor)
        {
            var bytes = Read_u8(source, ref localCursor, 2);

            return BitConverter.ToUInt16(bytes.ToArray());
        }

        public uint Read_u32(IEnumerable<byte> source,
         ref int localCursor)
        {
            var bytes = Read_u8(source, ref localCursor, 4);
            return BitConverter.ToUInt32(bytes.ToArray());
        }

        public ulong Read_u64(IEnumerable<byte> source,
         ref int localCursor)
        {
            var bytes = Read_u8(source, ref localCursor, 8);
            return BitConverter.ToUInt64(bytes.ToArray());
        }
        #endregion

    }
}
