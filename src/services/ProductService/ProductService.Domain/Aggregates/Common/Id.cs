using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.Common
{
	public sealed class Id : IValueObject
	{
		private readonly Guid _value;

		public Id(Guid value)
		{
			if (value == Guid.Empty)
				throw new ArgumentException($"{typeof(Id).FullName} cannot be empty");
			_value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(Id)))
				return false;

			return ((Id)obj)._value == _value;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static implicit operator Id(Guid value)
		{
			return new Id(value);
		}

		public static implicit operator Guid(Id id)
		{
			return id._value;
		}
	}
}
