using ProductService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Core.Models.ProductModel
{
    public sealed class TemplateId : ValueObject
    {
        private readonly Guid _value;

        public TemplateId(Guid value)
        {
            _value = value;
            if (_value == Guid.Empty)
                throw new ValueException($"{nameof(TemplateId)} is empty Guid.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is TemplateId templateId)
                return _value.Equals(templateId._value);
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
            return _value.ToString();
        }

        public static implicit operator TemplateId(Guid value)
        {
            return new TemplateId(value);
        }

        public static implicit operator Guid(TemplateId domainId)
        {
            return domainId._value;
        }
    }
}
