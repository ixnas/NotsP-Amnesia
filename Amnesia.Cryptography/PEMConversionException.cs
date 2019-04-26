using System;

namespace Amnesia.Cryptography
{
	public class PEMConversionException : Exception
	{
		public PEMConversionException()
		{
		}

		public PEMConversionException(string message) : base(message)
		{
		}

		public PEMConversionException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}