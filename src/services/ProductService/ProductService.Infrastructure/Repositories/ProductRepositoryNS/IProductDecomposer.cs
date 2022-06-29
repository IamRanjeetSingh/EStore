using ProductService.Domain.Aggregates.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS
{
	public interface IProductDecomposer
	{
		public DecomposedProduct Decompose(Product product);
	}
}
