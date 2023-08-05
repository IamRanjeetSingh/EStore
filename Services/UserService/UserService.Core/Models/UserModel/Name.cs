using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public sealed class Name : ValueObject
    {
        public string FirstName { get; }
        
        public string LastName { get; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            EnsureValuesAreValid();
        }

        private void EnsureValuesAreValid()
        {
            Regex firstNameRegex = new(@"^[a-zA-Z]+$");
            if (FirstName == null || !firstNameRegex.IsMatch(FirstName))
                throw new ValueException($"The provided FirstName value '{FirstName}' is invalid.");

            Regex lastNameRegex = new(@"^[a-zA-Z]+$");
            if (LastName == null || !lastNameRegex.IsMatch(LastName))
                throw new ValueException($"The provided LastName value '{LastName}' is invalid.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Name)
                return false;

            Name name = (Name)obj;
            return
                string.Equals(FirstName, name.FirstName) &&
                string.Equals(LastName, name.LastName);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(FirstName);
            hashCode.Add(LastName);
            return hashCode.ToHashCode();
        }
    }
}
