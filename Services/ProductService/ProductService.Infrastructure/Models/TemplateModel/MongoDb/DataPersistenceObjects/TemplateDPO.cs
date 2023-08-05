namespace ProductService.Infrastructure.Models.TemplateModel.MongoDb.DataPersistenceObjects
{
    internal sealed class TemplateDPO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TemplatePropertyDPO> Properties { get; set; }

#pragma warning disable CS8618
        public TemplateDPO() { }
#pragma warning restore CS8618
    }
}
