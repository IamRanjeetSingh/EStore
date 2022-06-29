using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries.GetProductById
{
	public sealed class GetProductByIdQuery : IQuery
	{
		public Guid Id { get; set; }
	}
}
