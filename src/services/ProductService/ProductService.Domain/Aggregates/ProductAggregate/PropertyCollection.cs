using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public sealed class PropertyCollection : IEnumerable<Property>
	{
		private readonly Dictionary<string, Property> properties = new();

		public void Add(Property property)
		{
			properties.Add(property.Name, property);
		}

		public IEnumerator<Property> GetEnumerator()
		{
			return properties.Select(keyValuePair => keyValuePair.Value).GetEnumerator();
		}

		public void RemoveByName(string propertyName)
		{
			properties.Remove(propertyName);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
