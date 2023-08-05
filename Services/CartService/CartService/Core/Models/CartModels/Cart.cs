using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public sealed class Cart
    {
        public CartId Id { get; }

        public Owner Owner { get; }

        public ProductCollection Products { get; }

        public Cart(Owner owner)
        {
            Id = new();
            Owner = owner;
            Products = new ProductCollection();
        }

        public Cart(Snapshot snapshot)
        {
            Id = new(snapshot.Id);
            Owner = new(snapshot.Owner);
            Products = new(snapshot.Products.Select(productSnapshot => new Product(productSnapshot)));
        }

        public Snapshot CreateSnapshot()
        {
            return new Snapshot(Id.Value, Owner.CreateSnapshot(), Products.Select(product => product.CreateSnapshot()));
        }

        public sealed class Snapshot
        {
            public Guid Id { get; }
            public Owner.Snapshot Owner { get; }
            public IEnumerable<Product.Snapshot> Products { get; }

            public Snapshot(Guid id, Owner.Snapshot owner, IEnumerable<Product.Snapshot> products)
            {
                Id = id;
                Owner = owner;
                Products = products;
            }
        }
    }
}
