using CartService.Core.Models.CartModels;
using CartService.Infra.Database.CartModels.DataPersistanceObjects;
using CartService.Infra.Utils.MongoDb;
using MongoDB.Driver;

namespace CartService.Infra.Database.CartModels
{
    public sealed class CartMongoDbDAO : ICartDAO
    {
        public class Options
        {
            public string? ConnectionString { get; init; }
            public string? DatabaseName { get; init; }
            public string? CollectionName { get; init; }
        }

        private readonly IMongoCollection<CartDPO> _cartItemCollection;

        public CartMongoDbDAO(Options options)
        {
            EnsureOptionsAreValid(options);
            IMongoClient client = new MongoClient(options.ConnectionString);
            IMongoDatabase database = client.GetDatabase(options.DatabaseName);
            _cartItemCollection = database.GetCollection<CartDPO>(options.CollectionName);
            GuidSerializerRegistrar.Register();
        }

        private void EnsureOptionsAreValid(Options options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new ArgumentException($"{nameof(Options)} {nameof(Options.ConnectionString)} is null or empty.");
            if (string.IsNullOrEmpty(options.DatabaseName))
                throw new ArgumentException($"{nameof(Options)} {nameof(Options.DatabaseName)} is null or empty.");
            if (string.IsNullOrEmpty(options.CollectionName))
                throw new ArgumentException($"{nameof(Options)} {nameof(Options.CollectionName)} is null or empty.");
        }

        public Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            CartDPO cartDPO = CreateDPOFromModel(cart);
            return AddToDatabaseAsync(cartDPO, cancellationToken);
        }

        public Task AddOrUpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            CartDPO cartDPO = CreateDPOFromModel(cart);
            return AddOrUpdateToDatabaseAsync(cartDPO, cancellationToken);
        }

        internal CartDPO CreateDPOFromModel(Cart cart)
        {
            Cart.Snapshot snapshot = cart.CreateSnapshot();
            return new CartDPO()
            {
                Id = snapshot.Id,
                Owner = new OwnerDPO()
                {
                    Id = snapshot.Owner.Id
                },
                Products = new List<ProductDPO>(snapshot.Products.Select(productSnapshot => new ProductDPO()
                {
                    Id = productSnapshot.Id
                }))
            };
        }

        private Task AddToDatabaseAsync(CartDPO cartDPO, CancellationToken cancellationToken = default)
        {
            return _cartItemCollection.InsertOneAsync(cartDPO, options: null, cancellationToken);
        }

        private Task AddOrUpdateToDatabaseAsync(CartDPO cartDPO, CancellationToken cancellationToken = default)
        {
            FilterDefinition<CartDPO> getByIdFilter = new FilterDefinitionBuilder<CartDPO>()
                .Eq(cartDPO => cartDPO.Id, cartDPO.Id);

            ReplaceOptions options = new()
            {
                IsUpsert = true
            };

            return _cartItemCollection.ReplaceOneAsync(getByIdFilter, cartDPO, options, cancellationToken);
        }

        public async Task<Cart?> GetByCartIdAsync(CartId id, CancellationToken cancellationToken = default)
        {
            CartDPO? cartDPO = await GetFromDatabaseAsync(id.Value, cancellationToken);
            return CreateModelFromDPO(cartDPO);
        }

        internal async Task<CartDPO?> GetFromDatabaseAsync(Guid id, CancellationToken cancellationToken = default)
        {
            FilterDefinition<CartDPO> getByIdFilter = new FilterDefinitionBuilder<CartDPO>()
                .Eq(cart => cart.Id, id);

            IAsyncCursor<CartDPO> cursor = await _cartItemCollection.FindAsync(getByIdFilter, options: null, cancellationToken);
            CartDPO? cartDPO = await cursor.FirstOrDefaultAsync(cancellationToken);

            return cartDPO;
        }

        internal Cart? CreateModelFromDPO(CartDPO? cartDPO)
        {
            if (cartDPO == null)
                return null;

            Cart.Snapshot snapshot = new(
                id: cartDPO.Id!.Value,
                owner: new Owner.Snapshot(id: cartDPO.Owner!.Id!),
                products: cartDPO.Products!.Select(productDPO => new Product.Snapshot(productDPO.Id!)));

            return new Cart(snapshot);
        }

        public Task<bool> RemoveByCartIdAsync(CartId id, CancellationToken cancellationToken = default)
        {
            return RemoveFromDatabaseAsync(id.Value, cancellationToken);
        }

        internal async Task<bool> RemoveFromDatabaseAsync(Guid id, CancellationToken cancellationToken = default)
        {
            FilterDefinition<CartDPO> deleteByIdFilter = new FilterDefinitionBuilder<CartDPO>()
                .Eq(cart => cart.Id, id);

            DeleteResult deleteResult = await _cartItemCollection.DeleteOneAsync(deleteByIdFilter, options: null, cancellationToken);

            return deleteResult.DeletedCount > 0;
        }

        public Task<Cart?> GetByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}