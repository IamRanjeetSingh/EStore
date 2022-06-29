using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.UpdateProduct
{
	public sealed class PropertyUpdateDetails
	{
		public string Name { get; set; }
		public List<string> Values { get; set; }
	}
}
