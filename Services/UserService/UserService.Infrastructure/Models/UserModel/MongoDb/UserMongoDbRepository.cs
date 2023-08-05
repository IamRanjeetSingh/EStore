using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserService.Core.Models.UserModel;
using UserService.Infrastructure.Models.UserModel.MongoDb.DataPersistenceObjects;

namespace UserService.Infrastructure.Models.UserModel.MongoDb
{
    internal sealed class UserMongoDbRepository : UserRepository
    {
        private readonly UserMongoDbDao _dao;
        private readonly ILogger<UserMongoDbRepository>? _logger;

        public UserMongoDbRepository(UserMongoDbDao dao, ILogger<UserMongoDbRepository>? logger = null, 
            ILogger<UserRepository>? baseLogger = null) : base(baseLogger)
        {
            _dao = dao;
            _logger = logger;
        }

        protected override Task<bool> IsHandleDuplicate(Handle handle, CancellationToken cancellationToken = default)
        {
            return _dao.IsHandleDuplicate(handle, cancellationToken); 
        }

        protected async override Task<Guid> AddInternalAsync(User user, CancellationToken cancellationToken = default)
        {
            UserDPO userDPO = CreateDPOFromSnapshot(user.GetSnapshot());
            _logger?.LogDebug("Adding UserDPO with id '{id}' via DAO.", userDPO.Id);
            string id = await _dao.AddAsync(userDPO, cancellationToken);
            Guid guidId = Guid.Parse(id);
            return guidId;
        }

        private UserDPO CreateDPOFromSnapshot(User.Snapshot snapshot)
        {
            _logger?.LogDebug("Creating UserDPO from Snapshot.");
            UserDPO userDPO = new()
            {
                Id = snapshot.Id.ToString(),
                Handle = snapshot.Handle,
                Email = snapshot.Email,
                EmailIsVerified = snapshot.EmailIsVerified,
                FirstName = snapshot.FirstName,
                LastName = snapshot.LastName,
                DateOfBirth = snapshot.DateOfBirth.ToString(),
                CreationDate = snapshot.CreationDate.ToString()
            };
            return userDPO;
        }

        protected async override Task<User?> GetInternalAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting User with id '{id}' via DAO.", id);
            UserDPO? userDPO = await _dao.GetByIdAsync(id.ToString(), cancellationToken);
            if (userDPO == null)
            {
                _logger?.LogDebug("User with id '{id}' not found via DAO.", id);
                return null;
            }
            User.Snapshot snapshot = CreateSnapshotFromDPO(userDPO);
            User user = new(snapshot);
            return user;
        }

        protected override async Task<User?> GetInternalAsync(Handle handle, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting User with handle '{handle}' via DAO.", handle);
            UserDPO? userDPO = await _dao.GetByHandleAsync(handle.ToString(), cancellationToken);
            if(userDPO == null)
            {
                _logger?.LogDebug("User with handle '{handle}' not found via DAO.", handle);
                return null;
            }
            User.Snapshot snapshot = CreateSnapshotFromDPO(userDPO);
            User user = new(snapshot);
            return user;
        }

        private User.Snapshot CreateSnapshotFromDPO(UserDPO userDPO)
        {
            User.Snapshot snapshot = new(
                Guid.Parse(userDPO.Id),
                userDPO.Handle,
                userDPO.Email,
                userDPO.EmailIsVerified,
                userDPO.FirstName,
                userDPO.LastName,
                DateOnly.Parse(userDPO.DateOfBirth),
                DateTimeOffset.Parse(userDPO.CreationDate));
            return snapshot;
        }

        protected async override Task<bool> RemoveInternalAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Removing User with id '{id}' via DAO.", id);
            bool wasFound = await _dao.RemoveAsync(id.ToString(), cancellationToken);
            if (wasFound)
                _logger?.LogDebug("User with id '{id}' not found via DAO.", id);
            return wasFound;
        }
    }
}
