using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public sealed class Price: IValueObject
	{
		public const int Precision = 2;

		private readonly double _value;

		public Price(double value)
		{
			_value = Math.Round(value, Precision);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(Price)))
				return false;
			return ((Price)obj)._value == _value;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static implicit operator Price(double value)
		{
			return new Price(value);
		}

		public static implicit operator double(Price price)
		{
			return price._value;
		}
	}
}
