using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infra.Database.CartModels.DataPersistanceObjects
{
    internal sealed class CartDPO
    {
        public Guid? Id { get; set; }

        public OwnerDPO? Owner { get; set; }

        public IEnumerable<ProductDPO>? Products { get; set; }
    }
}
