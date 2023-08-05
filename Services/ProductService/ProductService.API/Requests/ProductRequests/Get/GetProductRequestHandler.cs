using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;
using ProductService.Core.Models.ProductModel;
using CoreModels = ProductService.Core.Models.ProductModel;
using SchemaModels = ProductService.API.Schemas;

namespace ProductService.API.Requests.ProductRequests.Get
{
    internal sealed class GetProductRequestHandler : IRequestHandler<GetProductRequest, IActionResult>
    {
        private readonly IProductRepository _repsoitory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetProductRequestHandler>? _logger;

        public GetProductRequestHandler(IProductRepository repository, IHttpContextAccessor httpContextAccessor,
            ILogger<GetProductRequestHandler>? logger = null)
        {
            _repsoitory = repository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            CoreModels.Product? product = await _repsoitory.GetAsync(request.Id, cancellationToken);
            if (product == null)
            {
                _logger?.LogDebug("No Product found by id '{id}', returning 404 response.", request.Id);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "PRODUCT_NOT_FOUND",
                    message: $"No Product found by id '{request.Id}'."));
            }

            SchemaModels.Product? productSchema = CreateProductSchema(product);

            return new OkObjectResult(productSchema);
        }

        private SchemaModels.Product? CreateProductSchema(CoreModels.Product product)
        {
            _logger?.LogDebug("Creating response schema from Product.");
            SchemaModels.Product productSchema = new(
                id: product.Id,
                name: product.Name,
                templateId: product.TemplateId,
                properties: product.Properties.Select(property => new SchemaModels.ProductProperty(
                    name: property.Name,
                    value: property.Value)));
            return productSchema;
        }
    }
}
