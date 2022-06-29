using ProductService.Domain.Aggregates.Common;
using ProductService.Domain.Aggregates.ProductAggregate;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS
{
	public sealed class ProductIntegrator : IProductIntegrator
	{
		public Product Integrate(DecomposedProduct decomposedProduct)
		{
			PropertyCollection properties = new();
			List<Property> propertyList = decomposedProduct.PersistentProperties
				.Select(prop =>
				{
					List<string> values = new();
					decomposedProduct.PersistentPropertyValues
						.Where(propVal => propVal.PropertyName == prop.Name).ToList()
						.ForEach(propVal => values.AddRange(Enumerable.Repeat(propVal.Value, propVal.Count)));
					return Property.Create(prop.Name, values);
				})
				.ToList();
			propertyList.ForEach(prop => properties.Add(prop));

			return new ProductSubclass(
				decomposedProduct.PersistentProduct.Id,
				decomposedProduct.PersistentProduct.Title,
				decomposedProduct.PersistentProduct.Description,
				decomposedProduct.PersistentProduct.Price,
				decomposedProduct.PersistentProduct.CreatedOn,
				properties);
		}

		private sealed class ProductSubclass : Product
		{
			internal ProductSubclass(Id id, Title title, Description description, Price price, DateTime createdOn, PropertyCollection properties) : base(id, title, description, price, createdOn, properties)
			{
			}
		}
	}
}
