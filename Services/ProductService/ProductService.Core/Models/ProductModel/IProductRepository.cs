namespace ProductService.Core.Models.ProductModel
{
    public interface IProductRepository
    {
        public Task<Guid> AddAsync(Product product, CancellationToken cancellationToken = default);

        public Task<Product?> GetAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
