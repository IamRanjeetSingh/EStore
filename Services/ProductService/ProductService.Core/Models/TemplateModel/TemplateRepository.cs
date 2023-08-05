using Microsoft.Extensions.Logging;

namespace ProductService.Core.Models.TemplateModel
{
    public abstract class TemplateRepository : ITemplateRepository
    {
        private readonly ILogger<TemplateRepository>? _logger;

        public TemplateRepository(ILogger<TemplateRepository>? logger = null)
        {
            _logger = logger;
        }

        public Task<Guid> AddAsync(Template template, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Adding Template with id '{id}' to repository.", template.Id);
            return AddInternalAsync(template, cancellationToken);
        }

        protected abstract Task<Guid> AddInternalAsync(Template template, CancellationToken cancellationToken = default);

        public async Task<Template?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Getting Template with id '{id}' from repository.", id);
            Template? template = await GetInternalAsync(id, cancellationToken);
            if (template == null)
                _logger?.LogInformation("No Template with id '{id}' found in repository.", id);
            return template;
        }

        protected abstract Task<Template?> GetInternalAsync(Guid id, CancellationToken cancellationToken = default);

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Removing Template with id '{id}' from repository.", id);
            bool wasFound = await RemoveInternalAsync(id, cancellationToken);
            if (!wasFound)
                _logger?.LogInformation("No Template with id '{id}' found in repository.", id);
            return wasFound;
        }

        protected abstract Task<bool> RemoveInternalAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
