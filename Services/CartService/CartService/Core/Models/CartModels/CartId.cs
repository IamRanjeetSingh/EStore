﻿using CartService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public sealed class CartId
    {
        public Guid Value { get; }
        
        public CartId(Guid value)
        {
            Value = value;
            if (Value == Guid.Empty)
                throw new ValueException($"{nameof(CartId)} is empty guid.");
        }

        public CartId()
        {
            Value = Guid.NewGuid();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CartId cartId)
                return false;

            return Guid.Equals(Value, cartId.Value);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);
            return hashCode.ToHashCode();
        }
    }
}
