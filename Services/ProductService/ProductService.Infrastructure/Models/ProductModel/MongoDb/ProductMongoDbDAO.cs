using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProductService.Infrastructure.Models.ProductModel.MongoDb.DataPersistenceObjects;
using ProductService.Infrastructure.Utils.MongoDb;

namespace ProductService.Infrastructure.Models.ProductModel.MongoDb
{
    internal sealed class ProductMongoDbDAO : IProductMongoDbDAO
    {
        internal sealed class Options
        {
#pragma warning disable CS8618
            public string ConnectionString { get; init; }
            public string DatabaseName { get; init; }
            public string ProductCollectionName { get; init; }
#pragma warning restore CS8618
        }

        private readonly IMongoCollection<ProductDPO> _productCollection;
        private readonly ILogger<ProductMongoDbDAO>? _logger;

        public ProductMongoDbDAO(Options options, ILogger<ProductMongoDbDAO>? logger = null)
        {
            IMongoClient client = new MongoClient(options.ConnectionString);
            IMongoDatabase database = client.GetDatabase(options.DatabaseName);
            _productCollection = database.GetCollection<ProductDPO>(options.ProductCollectionName);
        }

        static ProductMongoDbDAO()
        {
            BsonSerializer.TryRegisterSerializer(GuidStringRepresentationSerializerProvider.Instance);
        }

        public async Task<Guid> AddAsync(ProductDPO product, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Adding Product with id '{id}' to database.", product.Id);
            await _productCollection.InsertOneAsync(product, options: null, cancellationToken);
            return product.Id;
        }

        public async Task<ProductDPO?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting Product with id '{id}' from database.", id);
            FilterDefinition<ProductDPO> getByIdFilter = new FilterDefinitionBuilder<ProductDPO>()
                .Eq(dpo => dpo.Id, id);
            IAsyncCursor<ProductDPO> cursor = await _productCollection.FindAsync(getByIdFilter, options: null, cancellationToken);
            ProductDPO? product = cursor.FirstOrDefault(cancellationToken);
            if (product == null)
                _logger?.LogDebug("Product with id '{id}' not found in database.", id);
            return product;
        }

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Removing Product with id '{id}' from database.", id);
            FilterDefinition<ProductDPO> deleteByIdFilter = new FilterDefinitionBuilder<ProductDPO>()
                .Eq(product => product.Id, id);
            DeleteResult deleteResult = await _productCollection.DeleteOneAsync(deleteByIdFilter, options: null, cancellationToken);
            if (deleteResult.DeletedCount == 0)
            {
                _logger?.LogDebug("Product with id '{id}' not found in database.", id);
                return false;
            }
            return true;
        }
    }
}
