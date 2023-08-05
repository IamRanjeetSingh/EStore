using Microsoft.Extensions.Logging;
using ProductService.Core.Exceptions;

namespace ProductService.Core.Models.Common
{
    public sealed class Name : ValueObject
    {
        private readonly string _value;

        public Name(string value)
        {
            _value = value;
            if (string.IsNullOrEmpty(_value))
                throw new ValueException($"{nameof(Name)} is null or empty.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is Name name)
                return string.Equals(name._value, _value);
            return false;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(_value);
            return hashCode.ToHashCode();
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator Name(string value)
        {
            return new Name(value);
        }

        public static implicit operator string(Name name)
        {
            return name._value;
        }
    }
}
