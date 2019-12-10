using System;

namespace FastBitmap
{
	[Serializable]
	public class BadStateException : Exception
	{
		public BadStateException()
		{
		}

		public BadStateException(string message) : base(message)
		{
		}

		public BadStateException(string message,Exception innerException) : base(message,innerException)
		{
		}
	}
}
