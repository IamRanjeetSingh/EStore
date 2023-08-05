using Microsoft.Extensions.Logging;
using ProductService.Core.Models.TemplateModel;
using ProductService.Infrastructure.Models.TemplateModel.MongoDb.DataPersistenceObjects;

namespace ProductService.Infrastructure.Models.TemplateModel.MongoDb
{
    internal sealed class TemplateMongoDbRepository : TemplateRepository
    {
        private readonly ITemplateMongoDbDAO _dao;
        private readonly ILogger<TemplateMongoDbRepository>? _logger;

        public TemplateMongoDbRepository(ITemplateMongoDbDAO dao, ILogger<TemplateMongoDbRepository>? logger = null,
            ILogger<TemplateRepository>? repositoryLogger = null) : base(repositoryLogger)
        {
            _dao = dao;
            _logger = logger;
        }

        protected override Task<Guid> AddInternalAsync(Template template, CancellationToken cancellationToken = default)
        {
            TemplateDPO templateDPO = CreateDPOFromSnapshot(template.GetSnapshot());
            _logger?.LogDebug("Adding Template with id '{id}' via DAO.", template.Id);
            return _dao.AddAsync(templateDPO, cancellationToken);
        }

        private TemplateDPO CreateDPOFromSnapshot(Template.Snapshot snapshot)
        {
            _logger?.LogDebug("Creating TemplateDPO from TemplateSnapshot.");
            TemplateDPO templateDPO = new()
            {
                Id = snapshot.Id,
                Name = snapshot.Name,
                Properties = snapshot.Properties.Select(property =>
                    new TemplatePropertyDPO()
                    {
                        Name = property.Name
                    })
            };
            return templateDPO;
        }

        protected override async Task<Template?> GetInternalAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting Template with id '{id}' via DAO.", id);
            TemplateDPO? templateDPO = await _dao.GetAsync(id, cancellationToken);
            if (templateDPO == null)
            {
                _logger?.LogDebug("Template with id '{id}' not found via DAO.", id);
                return null;
            }
            Template.Snapshot templateSnapshot = CreateSnapshotFromDPO(templateDPO);
            Template template = new(templateSnapshot);
            return template;
        }

        private Template.Snapshot CreateSnapshotFromDPO(TemplateDPO templateDPO)
        {
            _logger?.LogDebug("Creating TemplateSnapshot from TemplateDPO.");
            Template.Snapshot templateSnapshot = new(
                templateDPO.Id,
                templateDPO.Name,
                templateDPO.Properties.Select(templatePropertyDPO =>
                    new TemplateProperty(templatePropertyDPO.Name)));
            return templateSnapshot;
        }

        protected override async Task<bool> RemoveInternalAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Removing Template with id '{id}' via DAO.", id);
            bool wasFound = await _dao.RemoveAsync(id, cancellationToken);
            if (wasFound)
                _logger?.LogDebug("Template with id '{id}' not found via DAO.", id);
            return wasFound;
        }
    }
}
