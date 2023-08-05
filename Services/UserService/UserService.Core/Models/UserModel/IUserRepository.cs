using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Core.Exceptions;

namespace UserService.Core.Models.UserModel
{
    public interface IUserRepository
    {
        public Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default);

        public Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<User?> GetAsync(Handle handle, CancellationToken cancellationToken = default);

        public Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
