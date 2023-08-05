using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public abstract class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository>? _logger;

        public UserRepository(ILogger<UserRepository>? logger = null)
        {
            _logger = logger;
        }

        public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Adding User with id '{id}' to repository.", user.Id);
            if(await IsHandleDuplicate(user.Handle, cancellationToken))
            {
                _logger?.LogError("Handle '{handle}' already exists.", user.Handle);
                throw new ValueException($"Handle '{user.Handle}' already exists.");
            }

            return await AddInternalAsync(user, cancellationToken);
        }

        protected abstract Task<bool> IsHandleDuplicate(Handle handle, CancellationToken cancellationToken = default);

        protected abstract Task<Guid> AddInternalAsync(User user, CancellationToken cancellationToken = default);

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Getting User with id '{id}' from repository.", id);
            User? user = await GetInternalAsync(id, cancellationToken);
            if (user == null)
                _logger?.LogInformation("No User with id '{id}' found in repository.", id);
            return user;
        }

        protected abstract Task<User?> GetInternalAsync(Guid id, CancellationToken cancellationToken = default);

        public async Task<User?> GetAsync(Handle handle, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Getting User with handle '{handle}' from repository.", handle);
            User? user = await GetInternalAsync(handle, cancellationToken);
            if (user == null)
                _logger?.LogInformation("No User with handle '{handle}' found in repository.", handle);
            return user;
        }

        protected abstract Task<User?> GetInternalAsync(Handle handle, CancellationToken cancellationToken = default);

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Removing User with id '{id}' from repository.", id);
            bool wasFound = await RemoveInternalAsync(id, cancellationToken);
            if (!wasFound)
                _logger?.LogInformation("No User with id '{id}' found in repository.", id);
            return wasFound;
        }

        protected abstract Task<bool> RemoveInternalAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
