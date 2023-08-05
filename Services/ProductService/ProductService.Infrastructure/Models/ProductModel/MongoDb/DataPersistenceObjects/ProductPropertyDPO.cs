namespace ProductService.Infrastructure.Models.ProductModel.MongoDb.DataPersistenceObjects
{
    internal sealed class ProductPropertyDPO
    {
        public string Name { get; set; }

        public string Value { get; set; }

#pragma warning disable CS8618
        public ProductPropertyDPO() { }
#pragma warning restore CS8618
    }
}
