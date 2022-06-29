using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao
{
	public interface IPersistentProductDao
	{
		public Task<Guid> Insert(PersistentProduct persistentProduct);
		public Task<PersistentProduct> Get(Guid id);
		public Task<bool> Update(PersistentProduct persistentProduct);
		public Task<bool> Delete(Guid id);
	}
}
