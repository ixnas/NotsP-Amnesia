using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PemUtils;

namespace Amnesia.Cryptography
{
    public class KeyPair
    {
        public PublicKey PublicKey { get; private set; }
        public PrivateKey PrivateKey { get; private set; }

        /// <summary>
        /// Creates an RSA KeyPair using a private and a public key
        /// </summary>
        public KeyPair(PrivateKey privateKey, PublicKey publicKey)
        {
            this.PrivateKey = privateKey;
            this.PublicKey = publicKey;
        }

        /// <summary>
        /// Generates a new RSA KeyPair
        /// </summary>
        public KeyPair(int bits)
        {
            var cryptoServiceProvider = new RSACryptoServiceProvider(bits);
            PrivateKey = new PrivateKey(cryptoServiceProvider.ExportParameters(true));
            PublicKey = new PublicKey(cryptoServiceProvider.ExportParameters(false));
        }
    }
}
