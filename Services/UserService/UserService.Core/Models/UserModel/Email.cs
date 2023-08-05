using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public sealed class Email : ValueObject
    {
        public string Address { get; }
        
        public bool IsVerified { get; private set; }

        public Email(string value)
        {
            Address = value;
            EnsureAddressIsValid();
        }

        public Email(string value, bool isVerified) : this(value)
        {
            IsVerified = isVerified;
        }

        private void EnsureAddressIsValid()
        {
            Regex regex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (Address == null || !regex.IsMatch(Address))
                throw new ValueException($"The provided Email address '{Address}' is invalid.");
        }

        public void MarkAsVerified()
        {
            IsVerified = true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Email)
                return false;

            Email email = (Email)obj;

            return
                string.Equals(Address, email.Address) && 
                IsVerified == email.IsVerified;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Address);
            hashCode.Add(IsVerified);
            return hashCode.ToHashCode();
        }
    }
}
