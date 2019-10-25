using LibraAdmissionControlClient.Enum;
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
        public ScriptLCS Script { get;  set; }
        public ModuleLCS Module { get;  set; }

        public override string ToString()
        {
            string retStr ="{" + 
                string.Format("PayloadType = {0},{1}", PayloadTypeEnum, Environment.NewLine);

            if (PayloadTypeEnum == ETransactionPayloadLCS.Program)
            {
                retStr += string.Format(" Program = {0},", Program);
            }
            if (PayloadTypeEnum == ETransactionPayloadLCS.Script)
            {
                retStr += string.Format(" Script = {0},", Script);
            }
            if (PayloadTypeEnum == ETransactionPayloadLCS.Module)
            {
                retStr += string.Format(" Module = {0},", Module);
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
