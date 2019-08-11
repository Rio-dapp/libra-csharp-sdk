using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LibraAdmissionControlClient
{
    /// <summary>
    /// Implementation of the HMAC-based Extract-and-Expand Key Derivation Function according to https://tools.ietf.org/html/rfc5869.
    /// </summary>
    public sealed class Hkdf : IDisposable
    {
        private HMAC _hmac;
        private HashAlgorithmName _hashAlgorithm;

        private byte[] _tInfoN;
        private bool _disposed = false;

        public Hkdf(HashAlgorithmName hashAlgorithm)
        {
            switch (hashAlgorithm.Name)
            {
                case nameof(HashAlgorithmName.MD5):
                    HashLength = 128 / 8;
                    break;
                case nameof(HashAlgorithmName.SHA1):
                    HashLength = 160 / 8;
                    break;
                case nameof(HashAlgorithmName.SHA256):
                    HashLength = 256 / 8;
                    break;
                case nameof(HashAlgorithmName.SHA384):
                    HashLength = 384 / 8;
                    break;
                case nameof(HashAlgorithmName.SHA512):
                    HashLength = 512 / 8;
                    break;
                default:
                    throw new NotSupportedException($"The hash algorithm {hashAlgorithm} is not supported.");
            }

            _hashAlgorithm = hashAlgorithm;
        }

        public int HashLength { get; }


        /// <summary>
        /// Performs the HKDF-Extract function.
        /// </summary>
        /// <param name="ikm">The input keying material for HKDF-Extract.</param>
        /// <param name="salt">An optional salt value (a non-secret random value); if not provided, it is set to a string of HashLen zeros.</param>
        /// <returns>a pseudorandom key of <see cref="HashLength"/> bytes</returns>
        public byte[] Extract(byte[] ikm, byte[] salt = null)
        {
            if (ikm == null)
                throw new ArgumentNullException(nameof(ikm));
            if (salt == null)
                salt = new byte[HashLength];
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            InitializeHMAC(salt);

            return _hmac.ComputeHash(ikm);
        }

        /// <summary>
        /// Performs the HKDF-Expand function.
        /// </summary>
        /// <param name="prk">The pseudorandom key of at least HashLen bytes in size (usually, the output from the <see cref="Extract(byte[], byte[])"/>)</param>
        /// <param name="length">The length of output keying material in bytes (must be greater or equal to 0 and smaller or equal to 255*<see cref="HashLength"/>)</param>
        /// <param name="info">The optional context and application specific information</param>
        /// <returns>The output keying material</returns>
        public byte[] Expand(byte[] prk, int length, byte[] info = null)
        {
            if (prk == null)
                throw new ArgumentNullException(nameof(prk));
            if (prk.Length < HashLength)
                throw new ArgumentException($"The length of prk must be equal or greater than {HashLength} octets.", nameof(prk));
            if (length < 0 || length > 255 * HashLength)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (length == 0)
                return Array.Empty<byte>();
            if (info == null)
                info = Array.Empty<byte>();

            if (_tInfoN == null || _tInfoN.Length != HashLength + info.Length + sizeof(byte))
                _tInfoN = new byte[HashLength + info.Length + sizeof(byte)];

            InitializeHMAC(prk);

            var result = new byte[length];
            var offset = 0;

            var n = 1;
            var tInfoNOffset = HashLength;

            Array.Copy(info, 0, _tInfoN, HashLength, info.Length);

            while (true)
            {
                _tInfoN[HashLength + info.Length] = (byte)(n++);

                var hmac = _hmac.ComputeHash(_tInfoN, tInfoNOffset, _tInfoN.Length - tInfoNOffset);
                tInfoNOffset = 0;

                Array.Copy(hmac, 0, result, offset, Math.Min(result.Length - offset, HashLength));

                if ((offset += HashLength) >= length)
                    break;

                Array.Copy(hmac, 0, _tInfoN, 0, HashLength);
            }

            return result;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Hkdf"/>.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            if (_hmac != null)
                _hmac.Dispose();

            if (_tInfoN != null)
            {
                Array.Clear(_tInfoN, 0, _tInfoN.Length);
                _tInfoN = null;
            }

            _disposed = true;
        }

        private void InitializeHMAC(byte[] key)
        {
            if (_hmac != null)
            {
                _hmac.Key = key;
                return;
            }

            switch (_hashAlgorithm.Name)
            {
                case nameof(HashAlgorithmName.MD5):
                    _hmac = new HMACMD5(key);
                    break;
                case nameof(HashAlgorithmName.SHA1):
                    _hmac = new HMACSHA1(key);
                    break;
                case nameof(HashAlgorithmName.SHA256):
                    _hmac = new HMACSHA256(key);
                    break;
                case nameof(HashAlgorithmName.SHA384):
                    _hmac = new HMACSHA384(key);
                    break;
                case nameof(HashAlgorithmName.SHA512):
                    _hmac = new HMACSHA512(key);
                    break;
            }
        }
    }
}
