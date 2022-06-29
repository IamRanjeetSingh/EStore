using ProductService.Domain.Aggregates.ProductAggregate;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS
{
	public sealed class ProductDecomposer : IProductDecomposer
	{
		public DecomposedProduct Decompose(Product product)
		{
			PersistentProduct persistentProduct = new();
			
			persistentProduct.Id = product.Id;
			persistentProduct.Title = product.Title;
			persistentProduct.Description = product.Description;
			persistentProduct.Price = product.Price;
			persistentProduct.CreatedOn = product.CreatedOn;

			List<PersistentProperty> persistentProperties = new();
			List<PersistentPropertyValue> persistentPropertyValues = new();

			foreach(Property property in product.Properties)
			{
				PersistentProperty persistentProperty = new();
				persistentProperty.ProductId = product.Id;
				persistentProperty.Name = property.Name;
				persistentProperties.Add(persistentProperty);

				Dictionary<string, int> propertyValueCountMap = new();
				foreach(string value in property.Values)
				{
					if (propertyValueCountMap.ContainsKey(value))
						propertyValueCountMap[value]++;
					else
						propertyValueCountMap.Add(value, 1);
					
				}
				persistentPropertyValues.AddRange(propertyValueCountMap.Select(propertyValueCount => 
				{
					PersistentPropertyValue persistentPropertyValue = new();
					persistentPropertyValue.PropertyName = property.Name;
					persistentPropertyValue.ProductId = product.Id;
					persistentPropertyValue.Value = propertyValueCount.Key;
					persistentPropertyValue.Count = propertyValueCount.Value;
					return persistentPropertyValue;
				}));
			}

			return new DecomposedProduct(persistentProduct, persistentProperties, persistentPropertyValues);
		}
	}
}
