using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public sealed class Title : IValueObject
	{
		private readonly string _value;

		public Title(string value)
		{
			_value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(Title)))
				return false;
			return ((Title)obj)._value == _value;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static implicit operator Title(string value)
		{
			return new Title(value);
		}

		public static implicit operator string(Title title)
		{
			return title._value;
		}
	}
}
