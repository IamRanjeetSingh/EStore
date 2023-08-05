namespace CartService.Core.Models.CartModels
{
    public sealed class CartRepository
    {

        //TODO: Do we need DAO? Can't we just make the repository class abstract
        private readonly ICartDAO _dao;

        public CartRepository(ICartDAO dao)
        {
            _dao = dao;
        }

        public Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            return _dao.AddAsync(cart, cancellationToken);
        }

        public Task AddOrUpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            return _dao.AddOrUpdateAsync(cart, cancellationToken);
        }

        public Task<Cart?> GetByCartIdAsync(CartId cartId, CancellationToken cancellationToken = default)
        {
            return _dao.GetByCartIdAsync(cartId, cancellationToken);
        }

        public Task<Cart?> GetByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken = default)
        {
            return _dao.GetByOwnerIdAsync(ownerId, cancellationToken);
        }

        public Task<bool> RemoveByCartIdAsync(CartId cartId, CancellationToken cancellationToken = default)
        {
            return _dao.RemoveByCartIdAsync(cartId, cancellationToken);
        }
    }
}