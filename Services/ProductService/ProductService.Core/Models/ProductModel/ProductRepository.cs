using Microsoft.Extensions.Logging;

namespace ProductService.Core.Models.ProductModel
{
    public abstract class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository>? _logger;

        public ProductRepository(ILogger<ProductRepository>? logger = null)
        {
            _logger = logger;
        }

        public Task<Guid> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Adding Product with id '{id}' to repository.", product.Id);
            return AddInternalAsync(product, cancellationToken);
        }

        protected abstract Task<Guid> AddInternalAsync(Product product, CancellationToken cancellationToken = default);

        public async Task<Product?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Getting Product with id '{id}' from repository.", id);
            Product? product = await GetInternalAsync(id, cancellationToken);
            if (product == null)
                _logger?.LogInformation("No Product with id '{id}' found in repository.", id);
            return product;
        }

        protected abstract Task<Product?> GetInternalAsync(Guid id, CancellationToken cancellationToken = default);

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Removing Product with id '{id}' from repository.", id);
            bool wasFound = await RemoveInternalAsync(id, cancellationToken);
            if (!wasFound)
                _logger?.LogInformation("No Product with id '{id}' found in repository.", id);
            return wasFound;
        }

        protected abstract Task<bool> RemoveInternalAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
