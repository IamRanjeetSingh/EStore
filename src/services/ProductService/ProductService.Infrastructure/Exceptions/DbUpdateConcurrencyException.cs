using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Exceptions
{
	[Serializable]
	public class DbUpdateConcurrencyException : Exception
	{
		public DbUpdateConcurrencyException(Guid rowVersion) : 
			base($"Update/Delete operation failed because no record was found with matching row version.\nRow Version: {rowVersion}") { }

		protected DbUpdateConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
