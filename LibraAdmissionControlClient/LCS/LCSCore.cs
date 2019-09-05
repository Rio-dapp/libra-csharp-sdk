﻿using LibraAdmissionControlClient.LCS.LCSTypes;
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

        public static T LCSerialization<T>(this byte[] source, ref int cursor)
        {
            var type = typeof(T);
            if (type == typeof(AddressLCS))
            {
                return (T)Convert.ChangeType(
                    _serialization.GetAddress(source, ref cursor), typeof(T));
            }
            else if (type == typeof(ulong))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetU64(source, ref cursor), typeof(T));
            }
            else if (type == typeof(uint))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetU32(source, ref cursor), typeof(T));
            }
            else if (type == typeof(string))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetString(source, ref cursor), typeof(T));
            }
            else if (type == typeof(byte))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetByte(source, ref cursor), typeof(T));
            }
            else if (type == typeof(byte[]))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetByteArray(source, ref cursor), typeof(T));
            }
            else if (type == typeof(TransactionPayloadLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetTransactionPayload(source, ref cursor), typeof(T));
            }
            else if (type == typeof(ProgramLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetProgram(source, ref cursor), typeof(T));
            }
            else if (type == typeof(TransactionArgumentLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetTransactionArgument(source, ref cursor), typeof(T));
            }
            else if (type == typeof(List<TransactionArgumentLCS>))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetTransactionArguments(source, ref cursor), typeof(T));
            }
            else if (type == typeof(List<byte[]>))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetListByteArrays(source, ref cursor), typeof(T));
            }
            else if (type == typeof(WriteSetLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetWriteSet(source, ref cursor), typeof(T));
            }
            else if (type == typeof(WriteOpLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetWriteOp(source, ref cursor), typeof(T));
            }
            else if (type == typeof(AccessPathLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetAccessPath(source, ref cursor), typeof(T));
            }
            else if (type == typeof(bool))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetBool(source, ref cursor), typeof(T));
            }
            else if (type == typeof(AccountResourceLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetAccountResource(source, ref cursor), typeof(T));
            }
            else if (type == typeof(RawTransactionLCS))
            {
                return (T)Convert.ChangeType(
                  _serialization.GetRawTransaction(source, ref cursor), typeof(T));
            }

            
            throw new Exception("Unsupported type.");
        }



        #region TO DO Deserialization
        /// <summary>
        /// Libra Canonical Deserialization
        /// </summary>
        /// <returns></returns>
        public static byte[] LCDeserialization(ulong source)
        {
           return _deserialization.U64ToByte(source);
        } 
        #endregion
    }
}