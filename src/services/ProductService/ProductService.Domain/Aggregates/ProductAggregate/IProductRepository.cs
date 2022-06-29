using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public interface IProductRepository
	{
		public Task<Id> Add(Product product);
		public Task<Product> Get(Id id);
		public Task<bool> Update(Product product);
		public Task<bool> Remove(Id id);
	}
}
