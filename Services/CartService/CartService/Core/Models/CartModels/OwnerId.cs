using CartService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public sealed class OwnerId
    {
        public string Value { get; }

        public OwnerId(string value)
        {
            Value = value;
            if (string.IsNullOrEmpty(Value))
                throw new ValueException($"{nameof(OwnerId)} value is null or empty.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is not OwnerId ownerId)
                return false;

            return string.Equals(Value, ownerId.Value);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);
            return hashCode.ToHashCode();
        }
    }
}
