using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao
{
	public interface IPersistentPropertyValueDao
	{
		public Task Merge(Guid productId, IEnumerable<PersistentPropertyValue> persistentPropertyValues);
		public Task<IEnumerable<PersistentPropertyValue>> GetByProduct(Guid productId);
	}
}
