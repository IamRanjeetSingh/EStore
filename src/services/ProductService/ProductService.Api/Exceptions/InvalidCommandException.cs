using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ProductService.Api.Exceptions
{
	[Serializable]
	public class InvalidCommandException : Exception
	{
		public InvalidCommandException(string message) : base(message) { }
		public InvalidCommandException(IEnumerable<string> messages) : base(messages.Aggregate((msgs,msg) => msgs+"\n"+msg)) { }

		protected InvalidCommandException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
