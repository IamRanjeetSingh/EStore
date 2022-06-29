using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.CreateProduct
{
	public sealed class CreateProductCommand : ICommand
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public List<PropertyCreateDetails> Properties { get; set; }
	}
}
