using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public interface ICartDAO
    {
        public Task AddAsync(Cart cart, CancellationToken cancellationToken = default);

        public Task AddOrUpdateAsync(Cart cart, CancellationToken cancellationToken = default);

        public Task<Cart?> GetByCartIdAsync(CartId cartId, CancellationToken cancellationToken = default);

        public Task<Cart?> GetByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken = default);

        public Task<bool> RemoveByCartIdAsync(CartId cartId, CancellationToken cancellationToken = default);

        public Task<bool> RemoveByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken = default);
    }
}