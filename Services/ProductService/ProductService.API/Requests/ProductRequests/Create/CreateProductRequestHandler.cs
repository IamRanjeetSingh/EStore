using EStore.Common.Extensions;
using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Controllers;
using ProductService.Core.Exceptions;
using ProductService.Core.Services.ProductBuider;
using ProductService.Core.Models.ProductModel;
using SchemaModels = ProductService.API.Schemas;

namespace ProductService.API.Requests.ProductRequests.Create
{
    public sealed class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, IActionResult>
    {
        private readonly ProductBuilderService _creatorService;
        private readonly IProductRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateProductRequestHandler>? _logger;

        public CreateProductRequestHandler(ProductBuilderService creatorService, IProductRepository repository,
            IHttpContextAccessor httpContextAccessor, ILogger<CreateProductRequestHandler>? logger = null)
        {
            _creatorService = creatorService;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Product product = await CreateProductFromArgs(request.Args, cancellationToken);
                await _repository.AddAsync(product, cancellationToken);
                SchemaModels.Product productSchema = CreateProductSchema(product);

                return new CreatedAtActionResult(
                    actionName: nameof(ProductController.GetById),
                    controllerName: nameof(ProductController).RemoveSuffix("Controller"),
                    routeValues: new { productId = productSchema.Id },
                    value: productSchema);
            }
            catch(Exception ex) when (ex is ValueException || ex is ProductServiceException)
            {
                _logger?.LogError("Error while creating Product, Exception: \n{ex},\nreturning 400 response.", ex);
                return new BadRequestObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "BAD_REQUEST",
                    message: ex.Message));
            }
        }

        private async Task<Product> CreateProductFromArgs(SchemaModels.CreateProductArgs args, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Creating new Product from arguments.");
            ProductBuilderService.ProductCreationArgs createArgs = new(
                name: args.Name,
                templateId: args.TemplateId,
                properties: args.Properties.Select(propertyArgs => new ProductBuilderService.ProductPropertyCreationArgs(
                    name: propertyArgs.Name,
                    value: propertyArgs.Value)));
            Product product = await _creatorService.BuildAsync(createArgs, cancellationToken);
            return product;
        }

        private SchemaModels.Product CreateProductSchema(Product product)
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
