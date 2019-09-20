﻿using LibraAdmissionControlClient.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
   public class TransactionPayloadLCS
    {
        public uint PayloadType { get; set; }

        public ETransactionPayloadLCS PayloadTypeEnum { get {
                return (ETransactionPayloadLCS)PayloadType;
            } }

        public ProgramLCS Program { get; set; }
        public WriteSetLCS WriteSet { get; set; }
        public ScriptLCS Script { get; internal set; }
        public ModuleLCS Module { get; internal set; }

        public override string ToString()
        {
            string retStr ="{" + 
                string.Format("PayloadType = {0},{1}", PayloadTypeEnum, Environment.NewLine);

            if (PayloadTypeEnum == ETransactionPayloadLCS.Program)
            {
                retStr += string.Format(" Program = {0},", Program);
            }
            else if (PayloadTypeEnum == ETransactionPayloadLCS.WriteSet)
            {
                retStr += string.Format(" WriteSet = {0},", WriteSet);
            }
            retStr +="}";
            return retStr;
        }
    }
}
