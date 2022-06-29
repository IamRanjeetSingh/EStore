using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.Common
{
	public class AggregateRoot : Entity
	{
		public AggregateRoot(Id id) : base(id) { }
	}
}
