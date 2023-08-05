using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;
using ProductService.Core.Models.ProductModel;

namespace ProductService.API.Requests.ProductRequests.Delete
{
    internal sealed class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, IActionResult>
    {
        private readonly IProductRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DeleteProductRequestHandler>? _logger;

        public DeleteProductRequestHandler(IProductRepository repository, IHttpContextAccessor httpContextAccessor,
            ILogger<DeleteProductRequestHandler>? logger = null)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            bool wasFound = await _repository.RemoveAsync(request.Id, cancellationToken);
            if (!wasFound)
            {
                _logger?.LogDebug("No Product found by id '{id}', returning 404 response.", request.Id);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "PRODUCT_NOT_FOUND",
                    message: $"No Product found by id '{request.Id}'."));
            }

            return new OkResult();
        }
    }
}
