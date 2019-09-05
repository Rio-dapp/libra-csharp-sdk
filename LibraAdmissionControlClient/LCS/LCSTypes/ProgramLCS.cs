using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraAdmissionControlClient.LCS.LCSTypes
{
    public class ProgramLCS
    {
        public uint CodeStringLength { get; internal set; }
        public string CodeString { get; internal set; }
        public IEnumerable<TransactionArgumentLCS> TransactionArguments { get; internal set; }
        public List<byte[]> Modules { get; internal set; }

        public override string ToString()
        {
            string retStr = "{" + 
                string.Format("CodeStringLength = {0},{1}", CodeStringLength,
                Environment.NewLine);
            retStr += string.Format("CodeString = {0},{1}", CodeString,
                Environment.NewLine);
            retStr += "Arguments = [";
            foreach (var item in TransactionArguments)
            {
                retStr += item;
                if (item != TransactionArguments.Last())
                    retStr += string.Format(",{0}", Environment.NewLine);
            }
            retStr += string.Format("],{0}", Environment.NewLine);
            retStr += "Modules = [";
            foreach (var item in Modules)
            {
                retStr += item.ByteArryToString();
                if (item != Modules.Last())
                    retStr += string.Format(",{0}", Environment.NewLine);
            }
            retStr += "]";
            return retStr;
        }
    }
}
