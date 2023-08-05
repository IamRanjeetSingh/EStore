namespace ProductService.Core.Models.TemplateModel
{
    public interface ITemplateRepository
    {
        public Task<Guid> AddAsync(Template template, CancellationToken cancellationToken = default);

        public Task<Template?> GetAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
