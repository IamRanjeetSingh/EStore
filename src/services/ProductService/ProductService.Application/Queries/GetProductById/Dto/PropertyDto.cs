using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries.GetProductById.Dto
{
	public sealed class PropertyDto : IPropertyDto
	{
		public string Name { get; set; }
		public IEnumerable<string> Values { get; set; }
	}
}
