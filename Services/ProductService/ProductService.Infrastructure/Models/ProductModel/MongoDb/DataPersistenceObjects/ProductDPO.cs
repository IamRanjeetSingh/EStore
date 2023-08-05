namespace ProductService.Infrastructure.Models.ProductModel.MongoDb.DataPersistenceObjects
{
    internal sealed class ProductDPO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TemplateId { get; set; }
        public IEnumerable<ProductPropertyDPO> Properties { get; set; }

#pragma warning disable CS8618
        public ProductDPO() { }
#pragma warning restore CS8618
    }
}
