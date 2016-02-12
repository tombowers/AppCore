using System;
using System.Runtime.Serialization;

namespace Invisual.Rest
{
	[Serializable]
	public class Non200HttpStatusException : Exception
	{
		public Non200HttpStatusException()
		{
		}

		public Non200HttpStatusException(string message, int httpStatus) : base(message)
		{
			StatusCode = httpStatus;
		}

		public Non200HttpStatusException(string message, int httpStatus, Exception inner) : base(message, inner)
		{
			StatusCode = httpStatus;
		}

		protected Non200HttpStatusException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

		public int StatusCode { get; private set; }
	}
}
