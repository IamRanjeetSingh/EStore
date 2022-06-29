using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.CreateProduct
{
	public sealed class PropertyCreateDetails
	{
		public string Name { get; set; }
		public List<string> Values { get; set; }

		public PropertyCreateDetails()
		{
			Values = new List<string>();
		}
	}
}
