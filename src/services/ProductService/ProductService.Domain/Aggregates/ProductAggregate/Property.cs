using ProductService.Domain.Aggregates.Common;
using ProductService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public class Property : IValueObject
	{
		private readonly List<string> values = new();

		public string Name { get; private set; }
		public IEnumerable<string> Values { get => values; }
		
		protected Property(string name, IEnumerable<string> values)
		{
			Name = name;
			this.values.AddRange(values);
		}

		public static Property Create(string name, IEnumerable<string> values)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (values == null)
				throw new ArgumentNullException(nameof(name));
			if (!values.Any())
				throw new EmptyPropertyException(name);
			
			return new Property(name, values);
		}

		public void UpdateName(string name)
		{
			Name = name;
		}

		public void UpdateValues(IEnumerable<string> values)
		{
			this.values.Clear();
			this.values.AddRange(values);
		}
	}
}
