using Microsoft.Extensions.Logging;
using ProductService.Core.Exceptions;
using ProductService.Core.Models.Common;
using ProductService.Core.Models.TemplateModel;
using ProductService.Core.Services.ProductBuider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductModel = ProductService.Core.Models.ProductModel;
using TemplateModel = ProductService.Core.Models.TemplateModel;

namespace ProductService.Core.Services.ProductBuider
{
    public sealed class ProductBuilderService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ILogger<ProductBuilderService>? _logger;

        public sealed class ProductCreationArgs
        {
            public string Name { get; }
            public Guid TemplateId { get; }
            public IEnumerable<ProductPropertyCreationArgs> Properties { get; }

            public ProductCreationArgs(string name, Guid templateId, IEnumerable<ProductPropertyCreationArgs> properties)
            {
                Name = name;
                TemplateId = templateId;
                Properties = properties;
            }
        }

        public sealed class ProductPropertyCreationArgs
        {
            public string Name { get; }
            public string Value { get; }

            public ProductPropertyCreationArgs(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        public ProductBuilderService(TemplateModel.ITemplateRepository templateRepository, ILogger<ProductBuilderService>? logger = null)
        {
            _templateRepository = templateRepository;
            _logger = logger;
        }

        public async Task<ProductModel.Product> BuildAsync(ProductCreationArgs args, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Creating new Product.");
            TemplateModel.Template template = await GetTemplateByIdAsync(args.TemplateId, cancellationToken);
            ProductModel.ProductPropertyCollection productProperties = CreateProductProperties(args.Properties);
            CheckForMissingTemplateProperties(template.Properties, productProperties);
            ProductModel.Product product = new(
                args.Name,
                args.TemplateId,
                productProperties);
            return product;
        }

        private async Task<TemplateModel.Template> GetTemplateByIdAsync(Guid templateId, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting Template by id '{id}' for creating new Product.", templateId);
            TemplateModel.Template? template = await _templateRepository.GetAsync(templateId, cancellationToken);

            if (template == null)
            {
                _logger?.LogError("No Template found by id '{id}', cannot create new Product without Template.", templateId);
                throw new ValueException($"No Template found by id '{templateId}', cannot create new Product without Template.");
            }

            return template;
        }

        private ProductModel.ProductPropertyCollection CreateProductProperties(IEnumerable<ProductPropertyCreationArgs> properties)
        {
            ProductModel.ProductPropertyCollection productProperties = new(properties.Select(property =>
                new ProductModel.ProductProperty(property.Name, property.Value)));
            return productProperties;
        }

        private void CheckForMissingTemplateProperties(TemplateModel.TemplatePropertyCollection templateProperties,
            ProductModel.ProductPropertyCollection productProperties)
        {
            _logger?.LogDebug("Checking if any Template property is missing in Product.");
            foreach (TemplateModel.TemplateProperty templateProperty in templateProperties)
            {
                bool productHasTemplateProperty = productProperties.Any(productProperty =>
                    string.Equals(productProperty.Name, templateProperty.Name));

                if (!productHasTemplateProperty)
                {
                    _logger?.LogError("Template property '{templatePropertyName}' not found in Product properties.", templateProperty.Name);
                    throw new ValueException($"Template property '{templateProperty.Name}' not found in Product properties.");
                }
            }
        }
    }
}
