﻿using System;
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
            var payloadType = U32ToByte(source.PayloadType);
            retArr = retArr.Concat(payloadType).ToList();

            if (source.PayloadTypeEnum == Enum.ETransactionPayloadLCS.Program)
                retArr = retArr.Concat(LCSCore.LCDeserialize(source.Program)).ToList();
            else if (source.PayloadTypeEnum == Enum.ETransactionPayloadLCS.WriteSet)
                throw new Exception("WriteSet Not Supported.");
            else if (source.PayloadTypeEnum == Enum.ETransactionPayloadLCS.Script)
                retArr = retArr.Concat(LCSCore.LCDeserialize(source.Script)).ToList();
            else if (source.PayloadTypeEnum == Enum.ETransactionPayloadLCS.Module)
                retArr = retArr.Concat(LCSCore.LCDeserialize(source.Module)).ToList();

            return retArr.ToArray();
        }

        public byte[] ProgramToByte(ProgramLCS source)
        {
            List<byte> retArr = new List<byte>();
            var code = LCSCore.LCDeserialize(source.Code);
            retArr = retArr.Concat(code).ToList();
            var transactionArg = LCSCore.LCDeserialize(source.TransactionArguments);
            retArr = retArr.Concat(transactionArg).ToList();
            var modules = LCSCore.LCDeserialize(source.Modules);
            retArr = retArr.Concat(modules).ToList();
            return retArr.ToArray();
        }

        public byte[] ScriptToByte(ScriptLCS source)
        {
            List<byte> retArr = new List<byte>();
            var code = LCSCore.LCDeserialize(source.Code);
            retArr = retArr.Concat(code).ToList();
            var transactionArg = LCSCore.LCDeserialize(source.TransactionArguments);
            retArr = retArr.Concat(transactionArg).ToList();
            return retArr.ToArray();
        }

        public byte[] ModuleToByte(ModuleLCS source)
        {
            List<byte> retArr = new List<byte>();
            var code = LCSCore.LCDeserialize(source.Code);
            retArr = retArr.Concat(code).ToList();
            return retArr.ToArray();
        }

        public byte[] TransactionArgumentToByte(TransactionArgumentLCS source)
        {
            List<byte> retArr = new List<byte>();
            var argType = U32ToByte(source.ArgType);
            retArr = retArr.Concat(argType).ToList();

            if (source.ArgTypeEnum == Enum.ETransactionArgumentLCS.U64)
            {
                var arg = U64ToByte(source.U64);
                retArr = retArr.Concat(arg).ToList();
            }
            else if (source.ArgTypeEnum == Enum.ETransactionArgumentLCS.Address)
            {
                var arg = LCSCore.LCDeserialize(source.Address);
                retArr = retArr.Concat(arg).ToList();
            }
            else if (source.ArgTypeEnum == Enum.ETransactionArgumentLCS.String)
            {
                var arg = LCSCore.LCDeserialize(source.String);
                retArr = retArr.Concat(arg).ToList();
            }
            else if (source.ArgTypeEnum == Enum.ETransactionArgumentLCS.ByteArray)
            {
                var arg = LCSCore.LCDeserialize(source.ByteArray);
                retArr = retArr.Concat(arg).ToList();
            }

            return retArr.ToArray();
        }

        public byte[] ListTransactionArgumentToByte(List<TransactionArgumentLCS> source)
        {
            List<byte> retArr = new List<byte>();
            var len = U32ToByte((uint)source.Count);
            retArr = retArr.Concat(len).ToList();
            foreach (var item in source)
            {
                var arg = LCSCore.LCDeserialize(item);
                retArr = retArr.Concat(arg).ToList();
            }
            return retArr.ToArray();
        }

        public byte[] RawTransactionToByte(RawTransactionLCS source)
        {
            List<byte> retArr = new List<byte>();
            var sender = LCSCore.LCDeserialize(source.Sender);
            retArr = retArr.Concat(sender).ToList();

            var sn = U64ToByte(source.SequenceNumber);
            retArr = retArr.Concat(sn).ToList();

            var payload = LCSCore.LCDeserialize(source.TransactionPayload);
            retArr = retArr.Concat(payload).ToList();

            var maxGasAmount = U64ToByte(source.MaxGasAmount);
            retArr = retArr.Concat(maxGasAmount).ToList();

            var gasUnitPrice = U64ToByte(source.GasUnitPrice);
            retArr = retArr.Concat(gasUnitPrice).ToList();

            var expirationTime = U64ToByte(source.ExpirationTime);
            retArr = retArr.Concat(expirationTime).ToList();

            return retArr.ToArray();
        }
    }
}