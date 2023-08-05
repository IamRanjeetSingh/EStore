using ProductService.Core.Models.Common;

namespace ProductService.Core.Models.ProductModel
{
    public sealed class ProductProperty : ValueObject
    {
        public Name Name { get; }
        public ProductPropertyValue Value { get; }

        public ProductProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ProductProperty property)
                return string.Equals(property.Name, Name);
            return false;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Name);
            return hashCode.ToHashCode();
        }
    }
}
