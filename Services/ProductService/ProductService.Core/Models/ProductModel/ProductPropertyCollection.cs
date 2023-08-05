using Microsoft.Extensions.Logging;
using ProductService.Core.Exceptions;
using System.Collections;

namespace ProductService.Core.Models.ProductModel
{
    public sealed class ProductPropertyCollection : IEnumerable<ProductProperty>
    {
        private readonly IReadOnlyList<ProductProperty> _properties;
        private readonly ILogger<ProductPropertyCollection>? _logger;

        public ProductPropertyCollection(IEnumerable<ProductProperty> properties, ILogger<ProductPropertyCollection>? logger = null)
        {
            _logger = logger;
            if (IsNullOrEmpty(properties))
            {
                _logger?.LogError("Product properties are null or empty.");
                throw new ValueException("Product properties are null or empty.");
            }
            _properties = new List<ProductProperty>(properties).AsReadOnly();
        }

        private bool IsNullOrEmpty(IEnumerable<ProductProperty> properties)
        {
            return properties == null || !properties.Any();
        }

        public IEnumerator<ProductProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
