using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public sealed class CreationDate : ValueObject
    {
        private readonly DateTimeOffset _value;

        public CreationDate(DateTimeOffset value)
        {
            _value = value;
            EnsureValueIsValid();
        }

        private void EnsureValueIsValid()
        {
           if (DateTimeOffset.Now.Subtract(_value).Seconds < 0)
                throw new ValueException($"The provided CreationDate value '{_value}' is invalid, creation date cannot be in the future.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CreationDate)
                return false;

            CreationDate creationDate = (CreationDate)obj;
            return _value.Equals(creationDate);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(_value);
            return hashCode.ToHashCode();
        }

        public static implicit operator CreationDate(DateTimeOffset value)
        {
            return new CreationDate(value);
        }

        public static implicit operator DateTimeOffset(CreationDate creationDate)
        {
            return creationDate._value;
        }
    }
}
