using AutoMapper;
using ProductService.Domain.Aggregates.ProductAggregate;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS
{
	public sealed class DecomposedProduct
	{
		internal PersistentProduct PersistentProduct { get; }
		internal IEnumerable<PersistentProperty> PersistentProperties { get; }
		internal IEnumerable<PersistentPropertyValue> PersistentPropertyValues { get; }

		internal DecomposedProduct(PersistentProduct persistentProduct, IEnumerable<PersistentProperty> persistentProperties, IEnumerable<PersistentPropertyValue> persistentPropertyValues)
		{
			PersistentProduct = persistentProduct;
			PersistentProperties = persistentProperties;
			PersistentPropertyValues = persistentPropertyValues;
		}
	}
}
