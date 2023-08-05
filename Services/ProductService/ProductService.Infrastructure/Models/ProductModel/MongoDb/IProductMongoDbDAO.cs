using ProductService.Infrastructure.Models.ProductModel.MongoDb.DataPersistenceObjects;

namespace ProductService.Infrastructure.Models.ProductModel.MongoDb
{
    internal interface IProductMongoDbDAO
    {
        public Task<Guid> AddAsync(ProductDPO product, CancellationToken cancellationToken = default);

        public Task<ProductDPO?> GetAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
