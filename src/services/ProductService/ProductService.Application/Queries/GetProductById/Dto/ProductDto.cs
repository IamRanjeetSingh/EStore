using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries.GetProductById.Dto
{
	public sealed class ProductDto : IProductDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public DateTime CreatedOn { get; set; }
		public IEnumerable<IPropertyDto> Properties { get; set; }
	}
}
