using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries.GetProductById.Dto
{
	public interface IProductDto
	{
		public Guid Id { get; }
		public string Title { get; }
		public string Description { get; }
		public double Price { get; }
		public DateTime CreatedOn { get; }
		public IEnumerable<IPropertyDto> Properties { get; }
	}
}
