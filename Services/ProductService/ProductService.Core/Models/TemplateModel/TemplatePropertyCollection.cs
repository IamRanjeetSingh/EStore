using Microsoft.Extensions.Logging;
using ProductService.Core.Exceptions;
using System.Collections;

namespace ProductService.Core.Models.TemplateModel
{
    public sealed class TemplatePropertyCollection : IEnumerable<TemplateProperty>
    {
        private readonly IReadOnlyList<TemplateProperty> _properties;
        private readonly ILogger<TemplatePropertyCollection>? _logger;

        public TemplatePropertyCollection(IEnumerable<TemplateProperty> properties, ILogger<TemplatePropertyCollection>? logger = null)
        {
            _logger = logger;
            _properties = new List<TemplateProperty>(properties).AsReadOnly();
        }

        public IEnumerator<TemplateProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
