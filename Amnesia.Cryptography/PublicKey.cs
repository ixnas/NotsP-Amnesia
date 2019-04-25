using System.Security.Cryptography;
using PemUtils;

namespace Amnesia.Cryptography
{
	public class PublicKey : Key
	{
		/// <summary>
		/// Write RSA parameters to a PEM writer
		/// </summary>
		override protected void WriteKey(PemWriter writer, RSAParameters rsaParameters)
		{
			writer.WritePublicKey(rsaParameters);
		}

		/// <summary>
		/// Create a public key using RSAParameters
		/// </summary>
		public PublicKey(RSAParameters rsaParameters) : base(rsaParameters)
		{
		}

		/// <summary>
		/// Create a public key using a PEM string
		/// </summary>
		public PublicKey(string keyPEM) : base(keyPEM)
		{
		}

		/// <summary>
		/// Create PEM string using RSAParameters
		/// </summary>
		override public string ToPEMString() {
			return base.ToPEMString(rsaCryptoServiceProvider.ExportParameters(false));
		}

		/// <summary>
		/// Verify data using a SHA256 signature
		/// </summary>
		public bool VerifyData(byte[] data, byte[] signature)
		{
			return rsaCryptoServiceProvider.VerifyData(data, new SHA256CryptoServiceProvider(), signature);
		}
	}
}