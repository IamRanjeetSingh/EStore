using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Exceptions
{
	[Serializable]
	public class EmptyPropertyException : Exception
	{
		public EmptyPropertyException(string propertyName) : base($"{propertyName} property has empty values.") { }

		protected EmptyPropertyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
