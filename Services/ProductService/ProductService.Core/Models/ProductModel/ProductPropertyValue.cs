namespace ProductService.Core.Models.ProductModel
{
    public sealed class ProductPropertyValue : ValueObject
    {
        private readonly string _value;

        public ProductPropertyValue(string value)
        {
            _value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ProductPropertyValue propertyValue)
                return string.Equals(propertyValue._value, _value);
            return false;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(_value);
            return hashCode.ToHashCode();
        }

        public static implicit operator ProductPropertyValue(string value)
        {
            return new ProductPropertyValue(value);
        }

        public static implicit operator string(ProductPropertyValue propertyValue)
        {
            return propertyValue._value;
        }
    }
}
