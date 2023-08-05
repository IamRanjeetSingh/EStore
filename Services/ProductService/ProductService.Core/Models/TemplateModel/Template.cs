using ProductService.Core.Models.Common;

namespace ProductService.Core.Models.TemplateModel
{
    public sealed class Template : AggregateRoot
    {
        public Name Name { get; }
        public TemplatePropertyCollection Properties { get; }

        public Template(string name, IEnumerable<TemplateProperty> properties)
        {
            Name = name;
            Properties = new TemplatePropertyCollection(properties);
        }

        public Template(Snapshot snapshot) : base(snapshot.Id)
        {
            Name = snapshot.Name;
            Properties = new TemplatePropertyCollection(snapshot.Properties);
        }

        public Snapshot GetSnapshot()
        {
            return new Snapshot(Id, Name, Properties);
        }

        public sealed class Snapshot
        {
            public Guid Id { get; }
            public string Name { get; }
            public IEnumerable<TemplateProperty> Properties { get; }

            public Snapshot(Guid id, string name, IEnumerable<TemplateProperty> properties)
            {
                Id = id;
                Name = name;
                Properties = properties;
            }
        }
    }
}
