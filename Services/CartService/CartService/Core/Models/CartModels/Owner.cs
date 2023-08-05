using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public sealed class Owner
    {
        public OwnerId Id { get; }

        internal Owner(OwnerId id)
        {
            Id = id;
        }

        public Owner(Snapshot snapshot)
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
