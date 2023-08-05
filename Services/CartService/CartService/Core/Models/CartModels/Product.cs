using CartService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public class Product
    {
        public ProductId Id { get; }

        internal Product(ProductId id)
        {
            Id = id;
        }

        public Product(Snapshot snapshot)
        {
            Id = new(snapshot.Id);
        }

        public Snapshot CreateSnapshot()
        {
            return new Snapshot(Id.Value);
        }

        public sealed class Snapshot
        {
            public string Id { get; }

            public Snapshot(string id)
            {
                Id = id;
            }
        }
    }
}
