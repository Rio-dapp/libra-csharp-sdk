using LibraAdmissionControlClient.LCS.LCSTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS
{
    public static class LCSCore
    {
        static LibraCanonicalDeserialization _deserialization =
           new LibraCanonicalDeserialization();
        static LibraCanonicalSerialization _serialization =
            new LibraCanonicalSerialization();

        public static T LCDeserialize<T>(this byte[] source)
        {
            int cursor = 0;
            return source.LCDeserialize<T>(ref cursor);
        }

        public static T LCDeserialize<T>(this byte[] source, ref int cursor)
        {
            var type = typeof(T);
            if (type == typeof(AddressLCS))
            {
                return (T)Convert.ChangeType(
                    _deserialization.GetAddress(source, ref cursor), typeof(T));
            }
            else if (type == typeof(ulong))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetU64(source, ref cursor), typeof(T));
            }
            else if (type == typeof(uint))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetU32(source, ref cursor), typeof(T));
            }
            else if (type == typeof(string))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetString(source, ref cursor), typeof(T));
            }
            else if (type == typeof(byte))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetByte(source, ref cursor), typeof(T));
            }
            else if (type == typeof(byte[]))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetByteArray(source, ref cursor), typeof(T));
            }
            else if (type == typeof(TransactionPayloadLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetTransactionPayload(source, ref cursor), typeof(T));
            }
            else if (type == typeof(ProgramLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetProgram(source, ref cursor), typeof(T));
            }
            else if (type == typeof(ScriptLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetScript(source, ref cursor), typeof(T));
            }
            else if (type == typeof(ModuleLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetModule(source, ref cursor), typeof(T));
            }
            else if (type == typeof(TransactionArgumentLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetTransactionArgument(source, ref cursor), typeof(T));
            }
            else if (type == typeof(List<TransactionArgumentLCS>))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetTransactionArguments(source, ref cursor), typeof(T));
            }
            else if (type == typeof(List<byte[]>))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetListByteArrays(source, ref cursor), typeof(T));
            }
            else if (type == typeof(WriteSetLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetWriteSet(source, ref cursor), typeof(T));
            }
            else if (type == typeof(WriteOpLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetWriteOp(source, ref cursor), typeof(T));
            }
            else if (type == typeof(AccessPathLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetAccessPath(source, ref cursor), typeof(T));
            }
            else if (type == typeof(bool))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetBool(source, ref cursor), typeof(T));
            }
            else if (type == typeof(AccountEventLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetAccountEvent(source, ref cursor), typeof(T));
            }
            else if (type == typeof(AccountResourceLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetAccountResource(source, ref cursor), typeof(T));
            }
            else if (type == typeof(RawTransactionLCS))
            {
                return (T)Convert.ChangeType(
                  _deserialization.GetRawTransaction(source, ref cursor), typeof(T));
            }
            throw new Exception("Unsupported type.");
        }

        #region Serialization
        /// <summary>
        /// Libra Canonical Serialization
        /// </summary>
        /// <returns></returns>
        public static byte[] LCSerialize(object source)
        {
            var type = source.GetType();
            if (type == typeof(AddressLCS))
            {
                return _serialization.AddressToByte((AddressLCS)source);
            }
            else if (type == typeof(ulong))
            {
                return _serialization.U64ToByte((ulong)source);
            }
            else if (type == typeof(uint))
            {
                return _serialization.U32ToByte((uint)source);
            }
            else if (type == typeof(string))
            {
                return _serialization.StringToByte((string)source);
            }
            else if (type == typeof(byte[]))
            {
                return _serialization.ByteArrToByte((byte[])source);
            }
            else if (type == typeof(List<byte[]>))
            {
                return _serialization.ListByteArrToByte((List<byte[]>)source);
            }
            else if (type == typeof(bool))
            {
                return _serialization.BoolToByte((bool)source);
            }
            else if (type == typeof(TransactionPayloadLCS))
            {
                return _serialization.TransactionPayloadToByte(
                    (TransactionPayloadLCS)source);
            }
            else if (type == typeof(ProgramLCS))
            {
                return _serialization.ProgramToByte(
                    (ProgramLCS)source);
            }
            else if (type == typeof(ScriptLCS))
            {
                return _serialization.ScriptToByte(
                   (ScriptLCS)source);
            }
            else if (type == typeof(ModuleLCS))
            {
                return _serialization.ModuleToByte(
                    (ModuleLCS)source);
            }
            else if (type == typeof(TransactionArgumentLCS))
            {
                return _serialization.TransactionArgumentToByte(
                    (TransactionArgumentLCS)source);
            }
            else if (type == typeof(List<TransactionArgumentLCS>))
            {
                return _serialization.ListTransactionArgumentToByte(
                   (List<TransactionArgumentLCS>)source);
            }
            else if (type == typeof(RawTransactionLCS))
            {
                return _serialization.RawTransactionToByte(
                   (RawTransactionLCS)source);
            }

            throw new Exception("Unsupported type.");
        }
        #endregion
    }
}
