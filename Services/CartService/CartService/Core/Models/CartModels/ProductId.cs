using CartService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public sealed class ProductId
    {
        public string Value { get; }

        public ProductId(string value)
        {
            Value = value;
            if (string.IsNullOrEmpty(Value))
                throw new ValueException($"{nameof(ProductId)} is null or empty.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ProductId productId)
                return false;

            return string.Equals(Value, productId.Value);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);
            return hashCode.ToHashCode();
        }
    }
}
