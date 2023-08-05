namespace ProductService.Core.Models
{
    public class AggregateRoot : Entity
    {
        public AggregateRoot() : base() { }

        public AggregateRoot(Guid id) : base(id) { }
    }
}
