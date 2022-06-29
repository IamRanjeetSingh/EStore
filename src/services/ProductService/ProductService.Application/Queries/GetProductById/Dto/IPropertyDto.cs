using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries.GetProductById.Dto
{
	public interface IPropertyDto
	{
		public string Name { get; }
		public IEnumerable<string> Values { get; }
	}
}
