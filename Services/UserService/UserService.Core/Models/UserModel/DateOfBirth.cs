using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public sealed class DateOfBirth : ValueObject
    {
        private readonly DateOnly _value;

        public DateOfBirth(DateOnly value)
        {
            _value = value;
            EnsureValueIsValid();
        }

        private void EnsureValueIsValid()
        {
            DateTime valueAsDateTime = _value.ToDateTime(new TimeOnly(hour: 0, minute: 0));
            TimeSpan age = DateTime.Now.Subtract(valueAsDateTime);

            const int MaxTimeZoneDifferenceInHours = 27;
            if (age.TotalHours < -MaxTimeZoneDifferenceInHours)
                throw new ValueException($"The provided DateOfBirth value '{_value}' is invalid, date of birth cannot be in the future.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is not DateOfBirth)
                return false;

            DateOfBirth dateOfBirth = (DateOfBirth)obj;
            return _value.Equals(dateOfBirth);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(_value);
            return hashCode.ToHashCode();
        }

        public static implicit operator DateOfBirth(DateOnly value)
        {
            return new DateOfBirth(value);
        }

        public static implicit operator DateOnly(DateOfBirth dateOfBirth)
        {
            return dateOfBirth._value;
        }
    }
}
