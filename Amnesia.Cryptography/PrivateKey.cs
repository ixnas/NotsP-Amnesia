using System.Security.Cryptography;

namespace Amnesia.Cryptography
{
	class PrivateKey : Key
	{
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
			return base.ToPEMString(rsaCryptoServiceProvider.ExportParameters(true), true);
		}

		/// <summary>
		/// Sign data using a SHA256 signature
		/// </summary>
		public byte[] SignData(byte[] data)
		{
			return rsaCryptoServiceProvider.SignData(data, new SHA256CryptoServiceProvider());
		}
	}
}