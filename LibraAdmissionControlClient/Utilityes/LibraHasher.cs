using System.Text;
using System.Linq;
using Waher.Security.SHA3;

namespace LibraAdmissionControlClient.Utilityes
{
    public class LibraHasher
    {
        const string LIBRA_HASH_SUFFIX = "@@$$LIBRA$$@@";

        EHashType _eHashType;
        public LibraHasher()
        {

        }

        public LibraHasher(EHashType eHashType)
        {
            _eHashType = eHashType;
        }

        public byte[] GetHash(byte[] rawBytes)
        {
            return GetHash(rawBytes, _eHashType);
        }

        public byte[] GetHash(byte[] rawBytes, EHashType eHashType)
        {
            SHA3_256 sHA3_256 = new SHA3_256();
            _eHashType = eHashType;
            return sHA3_256.ComputeVariable(
                GetSaltHash(GetSalt(_eHashType))
                .Concat(rawBytes).ToArray());
        }

        private byte[] GetSaltHash(string prefix)
        {
            SHA3_256 sHA3_256 = new SHA3_256();
            return sHA3_256.ComputeVariable(
                Encoding.ASCII.GetBytes(prefix + LIBRA_HASH_SUFFIX));
        }

        private string GetSalt(EHashType eHashType)
        {
            if (eHashType == EHashType.RawTransaction)
                return "RawTransaction";
            return string.Empty;
        }
    }
}
