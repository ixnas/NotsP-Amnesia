using System.IO;
using System.Security.Cryptography;
using System.Text;
using PemUtils;

namespace Amnesia.Cryptography
{
	abstract class Key
	{
		protected RSACryptoServiceProvider rsaCryptoServiceProvider;

		/// <summary>
		/// Create RSAParameters using a PEM string
		/// </summary>
		RSAParameters ReadRSAParametersFromPEM(string pem)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(pem);
			MemoryStream stream = new MemoryStream(bytes);
			var reader = new PemReader(stream);
			return reader.ReadRsaKey();
		}

		/// <summary>
		/// Create PEM string using RSAParameters
		/// </summary>
		protected string ToPEMString(RSAParameters rsaParameters, bool isPrivate = false)
		{
			var stream = new MemoryStream();
			var writer = new PemWriter(stream);

			if (isPrivate)
			{
				writer.WritePrivateKey(rsaParameters);
			}
			else
			{
				writer.WritePublicKey(rsaParameters);
			}

			if (!stream.TryGetBuffer(out var bytes))
			{
				return null;
			}

			return Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// Create a key using RSAParameters
		/// </summary>
		public Key(RSAParameters rsaParameters)
		{
			this.rsaCryptoServiceProvider = new RSACryptoServiceProvider();
			rsaCryptoServiceProvider.ImportParameters(rsaParameters);
		}

		/// <summary>
		/// Create a key using a PEM string
		/// </summary>
		public Key(string keyPEM)
		{
			this.rsaCryptoServiceProvider = new RSACryptoServiceProvider();
			var rsaParameters = ReadRSAParametersFromPEM(keyPEM);
			rsaCryptoServiceProvider.ImportParameters(rsaParameters);
		}

		/// <summary>
		/// Returns the PEM string of the key
		/// </summary>
		public abstract string ToPEMString();
	}
}