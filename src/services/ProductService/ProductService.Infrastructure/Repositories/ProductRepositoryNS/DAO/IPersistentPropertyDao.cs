using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao
{
	public interface IPersistentPropertyDao
	{
		public Task Merge(Guid productId, IEnumerable<PersistentProperty> persistentProperties);
		public Task<IEnumerable<PersistentProperty>> GetByProduct(Guid productId);
	}
}
