using ProductService.Domain.Aggregates.Common;
using ProductService.Domain.Aggregates.ProductAggregate;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS
{
	public sealed class ProductRepositoryImpl : ProductRepository
	{
		private readonly IPersistentProductDao _persistentProductDao;
		private readonly IPersistentPropertyDao _persistentPropertyDao;
		private readonly IPersistentPropertyValueDao _persistentPropertyValueDao;
		private readonly IProductDecomposer _productDecomposer;
		private readonly IProductIntegrator _productIntegrator;

		public ProductRepositoryImpl(IPersistentProductDao persistentProductDao, IPersistentPropertyDao persistentPropertyDao, IPersistentPropertyValueDao persistentPropertyValueDao, IProductDecomposer productDecomposer, IProductIntegrator productIntegrator)
		{
			_persistentProductDao = persistentProductDao;
			_persistentPropertyDao = persistentPropertyDao;
			_persistentPropertyValueDao = persistentPropertyValueDao;
			_productDecomposer = productDecomposer;
			_productIntegrator = productIntegrator;
		}

		protected override async Task<Id> AddImpl(Product product)
		{
			DecomposedProduct decomposedProduct = _productDecomposer.Decompose(product);
			await _persistentProductDao.Insert(decomposedProduct.PersistentProduct);
			await _persistentPropertyDao.Merge(product.Id, decomposedProduct.PersistentProperties);
			await _persistentPropertyValueDao.Merge(product.Id, decomposedProduct.PersistentPropertyValues);

			return product.Id;
		}

		protected override async Task<Product> GetImpl(Id id)
		{
			PersistentProduct persistentProduct = await _persistentProductDao.Get(id);
			if (persistentProduct == null)
				return null;

			IEnumerable<PersistentProperty> persistentProperties = await _persistentPropertyDao.GetByProduct(id);
			IEnumerable<PersistentPropertyValue> persistentPropertyValues = await _persistentPropertyValueDao.GetByProduct(id);

			DecomposedProduct decomposedProduct = new(persistentProduct, persistentProperties, persistentPropertyValues);

			return _productIntegrator.Integrate(decomposedProduct);
		}

		protected async override Task<bool> UpdateImpl(Product product)
		{
			DecomposedProduct decomposedProduct = _productDecomposer.Decompose(product);
			bool isPersistentProductUpdated = await _persistentProductDao.Update(decomposedProduct.PersistentProduct);
			if (!isPersistentProductUpdated)
				return false;

			await _persistentPropertyDao.Merge(product.Id, decomposedProduct.PersistentProperties);
			await _persistentPropertyValueDao.Merge(product.Id, decomposedProduct.PersistentPropertyValues);
			
			return true;
		}

		protected override Task<bool> RemoveImpl(Id id)
		{
			return _persistentProductDao.Delete(id);
		}
	}
}
