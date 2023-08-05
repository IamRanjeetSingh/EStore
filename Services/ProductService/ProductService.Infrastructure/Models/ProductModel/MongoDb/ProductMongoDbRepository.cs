using Microsoft.Extensions.Logging;
using ProductService.Core.Models.ProductModel;
using ProductService.Infrastructure.Models.ProductModel.MongoDb.DataPersistenceObjects;

namespace ProductService.Infrastructure.Models.ProductModel.MongoDb
{
    internal sealed class ProductMongoDbRepository : ProductRepository
    {
        private readonly IProductMongoDbDAO _dao;
        private readonly ILogger<ProductMongoDbRepository>? _logger;

        public ProductMongoDbRepository(IProductMongoDbDAO dao, ILogger<ProductMongoDbRepository>? logger = null,
            ILogger<ProductRepository>? baseLogger = null) : base(baseLogger)
        {
            _dao = dao;
            _logger = logger;
        }

        protected override async Task<Guid> AddInternalAsync(Product product, CancellationToken cancellationToken = default)
        {
            ProductDPO productDPO = CreateDPOFromSnapshot(product.GetSnapshot());
            _logger?.LogDebug("Adding ProductDPO with id '{id}' via DAO.", productDPO.Id);
            Guid id = await _dao.AddAsync(productDPO, cancellationToken);
            return id;
        }

        private ProductDPO CreateDPOFromSnapshot(Product.Snapshot snapshot)
        {
            _logger?.LogDebug("Creating ProductDPO from ProductSnapshot.");
            ProductDPO productDPO = new()
            {
                Id = snapshot.Id,
                Name = snapshot.Name,
                TemplateId = snapshot.TemplateId,
                Properties = snapshot.Properties.Select(property =>
                    new ProductPropertyDPO()
                    {
                        Name = property.Name,
                        Value = property.Value
                    })
            };
            return productDPO;
        }

        protected override async Task<Product?> GetInternalAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting Product with id '{id}' via DAO.", id);
            ProductDPO? productDPO = await _dao.GetAsync(id, cancellationToken);
            if (productDPO == null)
            {
                _logger?.LogDebug("Product with id '{id}' not found via DAO.", id);
                return null;
            }
            Product.Snapshot productSnapshot = CreateSnapshotFromDPO(productDPO);
            Product product = new(productSnapshot);
            return product;
        }

        private Product.Snapshot CreateSnapshotFromDPO(ProductDPO productDPO)
        {
            Product.Snapshot productSnapshot = new(
                productDPO.Id,
                productDPO.Name,
                productDPO.TemplateId,
                productDPO.Properties.Select(propertyDPO =>
                    new ProductProperty(propertyDPO.Name, propertyDPO.Value)));
            return productSnapshot;
        }

        protected override async Task<bool> RemoveInternalAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Removing Product with id '{id}' via DAO.", id);
            bool wasFound = await _dao.RemoveAsync(id, cancellationToken);
            if (wasFound)
                _logger?.LogDebug("Product with id '{id}' not found via DAO.", id);
            return wasFound;
        }
    }
}

