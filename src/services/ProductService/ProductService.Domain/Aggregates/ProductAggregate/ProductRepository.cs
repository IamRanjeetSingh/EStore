using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public abstract class ProductRepository : IProductRepository
	{
		public Task<Id> Add(Product product)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));

			return AddImpl(product);
		}

		protected abstract Task<Id> AddImpl(Product product);

		public Task<Product> Get(Id id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			return GetImpl(id);
		}

		protected abstract Task<Product> GetImpl(Id id);

		public Task<bool> Update(Product product)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));

			return UpdateImpl(product);
		}

		protected abstract Task<bool> UpdateImpl(Product product);

		public Task<bool> Remove(Id id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			return RemoveImpl(id);
		}

		protected abstract Task<bool> RemoveImpl(Id id);
	}
}
