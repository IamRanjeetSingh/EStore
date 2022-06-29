using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public sealed class Description: IValueObject
	{
		private readonly string _value;

		public Description(string value)
		{
			_value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(Description)))
				return false;
			return ((Description)obj)._value == _value;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static implicit operator Description(string value)
		{
			return new Description(value);
		}

		public static implicit operator string(Description description)
		{
			return description._value;
		}
	}
}
