using ProductService.Infrastructure.Models.TemplateModel.MongoDb.DataPersistenceObjects;

namespace ProductService.Infrastructure.Models.TemplateModel.MongoDb
{
    internal interface ITemplateMongoDbDAO
    {
        public Task<Guid> AddAsync(TemplateDPO template, CancellationToken cancellationToken);

        public Task<TemplateDPO?> GetAsync(Guid id, CancellationToken cancellationToken);

        public Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}
