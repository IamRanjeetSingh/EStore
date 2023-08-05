using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProductService.Infrastructure.Models.TemplateModel.MongoDb.DataPersistenceObjects;
using ProductService.Infrastructure.Utils.MongoDb;

namespace ProductService.Infrastructure.Models.TemplateModel.MongoDb
{
    internal sealed class TemplateMongoDbDAO : ITemplateMongoDbDAO
    {
        internal sealed class Options
        {
#pragma warning disable CS8618
            public string ConnectionString { get; init; }
            public string DatabaseName { get; init; }
            public string TemplateCollectionName { get; init; }
#pragma warning restore CS8618
        }

        private readonly IMongoCollection<TemplateDPO> _templateCollection;
        private readonly ILogger<TemplateMongoDbDAO>? _logger;

        public TemplateMongoDbDAO(Options options, ILogger<TemplateMongoDbDAO>? logger = null)
        {
            IMongoClient client = new MongoClient(options.ConnectionString);
            IMongoDatabase database = client.GetDatabase(options.DatabaseName);
            _templateCollection = database.GetCollection<TemplateDPO>(options.TemplateCollectionName);
            _logger = logger;

        }

        static TemplateMongoDbDAO()
        {
            BsonSerializer.TryRegisterSerializer(GuidStringRepresentationSerializerProvider.Instance);
        }

        public async Task<Guid> AddAsync(TemplateDPO templateDPO, CancellationToken cancellationToken)
        {
            _logger?.LogDebug("Adding TemplateDPO with id '{id}' to database.", templateDPO.Id);
            await _templateCollection.InsertOneAsync(templateDPO, options: null, cancellationToken);
            return templateDPO.Id;
        }

        public async Task<TemplateDPO?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger?.LogDebug("Getting Template with id '{id}' from database.", id);
            FilterDefinition<TemplateDPO> getByIdFilter = new FilterDefinitionBuilder<TemplateDPO>()
                .Eq(dpo => dpo.Id, id);
            IAsyncCursor<TemplateDPO> cursor = await _templateCollection.FindAsync(getByIdFilter, options: null, cancellationToken);
            TemplateDPO? templateDPO = cursor.FirstOrDefault(cancellationToken);
            if (templateDPO == null)
                _logger?.LogDebug("Template with id '{id}' not found in database.", id);
            return templateDPO;
        }

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger?.LogDebug("Removing Template with id '{id}' from database.", id);
            FilterDefinition<TemplateDPO> deleteByIdFilter = new FilterDefinitionBuilder<TemplateDPO>()
                .Eq(templateDPO => templateDPO.Id, id);
            DeleteResult deleteResult = await _templateCollection.DeleteOneAsync(deleteByIdFilter, options: null, cancellationToken);
            if (deleteResult.DeletedCount == 0)
            {
                _logger?.LogDebug("Template with id '{id}' not found in database.", id);
                return false;
            }
            return true;
        }
    }
}
