using ProductService.Core.Models.Common;

namespace ProductService.Core.Models.ProductModel
{
    public sealed class Product : AggregateRoot
    {
        public Name Name { get; }
        public TemplateId TemplateId { get; }
        public ProductPropertyCollection Properties { get; }

        internal Product(string name, Guid templateId, IEnumerable<ProductProperty> properties)
        {
            Name = name;
            TemplateId = templateId;
            Properties = new ProductPropertyCollection(properties);
        }

        public Product(Snapshot snapshot) : base(snapshot.Id)
        {
            Name = snapshot.Name;
            TemplateId = snapshot.TemplateId;
            Properties = new ProductPropertyCollection(snapshot.Properties);
        }

        public Snapshot GetSnapshot()
        {
            return new Snapshot(Id, Name, TemplateId, Properties);
        }

        public sealed class Snapshot
        {
            public Guid Id { get; }
            public string Name { get; }
            public Guid TemplateId { get; }
            public IEnumerable<ProductProperty> Properties { get; }

            public Snapshot(Guid id, string name, Guid templateId, IEnumerable<ProductProperty> properties)
            {
                Id = id;
                Name = name;
                TemplateId = templateId;
                Properties = properties;
            }
        }
    }
}
