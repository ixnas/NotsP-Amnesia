using System.Security.Cryptography;
using PemUtils;

namespace Amnesia.Cryptography
{
    public class PrivateKey : Key
    {
        /// <summary>
        /// Write RSA parameters to a PEM writer
        /// </summary>
        override protected void WriteKey(PemWriter writer, RSAParameters rsaParameters)
        {
            writer.WritePrivateKey(rsaParameters);
        }

        /// <summary>
        /// Create a private key using RSAParameters
        /// </summary>
        public PrivateKey(RSAParameters rsaParameters) : base(rsaParameters)
        {
        }

        /// <summary>
        /// Create a private key using a PEM string
        /// </summary>
        public PrivateKey(string keyPEM) : base(keyPEM)
        {
        }

        /// <summary>
        /// Create PEM string using RSAParameters
        /// </summary>
        override public string ToPEMString() {
            return base.ToPEMString(rsaCryptoServiceProvider.ExportParameters(true));
        }

        /// <summary>
        /// Sign data using a SHA256 signature
        /// </summary>
        public byte[] SignData(byte[] data)
        {
            return rsaCryptoServiceProvider.SignData(data, new SHA256CryptoServiceProvider());
        }

        /// <summary>
        /// Sign an object using a SHA256 signature
        /// </summary>
        public byte[] SignObject(object obj)
        {
            return SignData(serializer.Serialize(obj));
        }
    }
}