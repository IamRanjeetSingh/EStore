using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public sealed class Handle : ValueObject
    {
        private const string ValidValueRegexPattern = @"^\w{4,36}$";
        private const string InvalidAliasValueExceptionMessage = 
            "The provided handle value '{0}' is invalid. Valid handle must meet the following criteria:\n" +
            "It should be more than 4 and less than 36 characters long.\n" +
            "It should only contain alphanumeric and underscore characters.";

        private readonly string _value;

        public Handle(string value)
        {
            _value = value;
            EnsureValueIsValid();
        }

        private void EnsureValueIsValid()
        {
            Regex regex = new(ValidValueRegexPattern);
            if (_value == null || !regex.IsMatch(_value))
                throw new ValueException(string.Format(InvalidAliasValueExceptionMessage, _value));
        }

        public override bool Equals(object? obj)
        {
            if (obj is Handle alias)
                return string.Equals(_value, alias._value);

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

        public static implicit operator Handle(string value)
        {
            return new Handle(value);
        }

        public static implicit operator string(Handle alias)
        {
            return alias._value;
        }
    }
}
