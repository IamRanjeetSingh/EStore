using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Infrastructure.Models.UserModel.MongoDb.DataPersistenceObjects;
using UserService.Infrastructure.Utils;

namespace UserService.Infrastructure.Models.UserModel.MongoDb
{
    internal sealed class UserMongoDbDao
    {
        internal sealed class Options
        {
#pragma warning disable CS8618
            public string ConnectionString { get; init; }
            public string DatabaseName { get; init; }
            public string UserCollectionName { get; init; }
#pragma warning restore CS8618
        }

        private readonly IMongoCollection<UserDPO> _userCollection;
        private readonly ILogger<UserMongoDbDao>? _logger;


        public UserMongoDbDao(Options options, IMongoDbClientFactory mongoDbClientFactory, ILogger<UserMongoDbDao>? logger = null)
        {
            EnsureOptionsAreValid(options);
            _userCollection = mongoDbClientFactory.GetUserCollection(options);
            _logger = logger;
        }

        private void EnsureOptionsAreValid(Options options)
        {
            if(options == null)
            {
                _logger?.LogError("{options} is null in UserMongoDbDao.", nameof(Options));
                throw new ArgumentNullException(nameof(options), $"{nameof(Options)} is null in UserMongoDbDao.");
            }

            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                _logger?.LogError("{property} is null or empty in UserMongoDbDao Options.", nameof(Options.ConnectionString));
                throw new ArgumentException($"{nameof(Options.ConnectionString)} is null or empty in UserMongoDbDao Options..");
            }

            if(string.IsNullOrEmpty(options.DatabaseName))
            {
                _logger?.LogError("{property} is null or empty in UserMongoDbDao Options.", nameof(Options.DatabaseName));
                throw new ArgumentException($"{nameof(Options.DatabaseName)} is null or empty in UserMongoDbDao Options.");
            }

            if(string.IsNullOrEmpty(options.UserCollectionName))
            {
                _logger?.LogError("{property} is null or empty in UserMongoDbDao Options.", nameof(Options.UserCollectionName));
                throw new ArgumentException($"{nameof(Options.UserCollectionName)} is null or empty in UserMongoDbDao Options.");
            }
        }

        internal async Task<bool> IsHandleDuplicate(string handle, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Checking if User with handle '{handle}' already exists in database.", handle);
            FilterDefinition<UserDPO> getByHandle = new FilterDefinitionBuilder<UserDPO>()
                .Eq(dpo => dpo.Handle, handle);
            IAsyncCursor<UserDPO> cursor = await _userCollection.FindAsync(getByHandle, options: null, cancellationToken);
            return cursor.Any(cancellationToken);
        }

        internal async Task<string> AddAsync(UserDPO user, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Adding User with id '{id}' to database.", user.Id);
            await _userCollection.InsertOneAsync(user, options: null, cancellationToken);
            return user.Id;
        }

        internal async Task<UserDPO?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting User with id '{id}' from database.", id);
            FilterDefinition<UserDPO> getByIdFilter = new FilterDefinitionBuilder<UserDPO>()
                .Eq(dpo => dpo.Id, id);
            IAsyncCursor<UserDPO> cursor = await _userCollection.FindAsync(getByIdFilter, options: null, cancellationToken);
            UserDPO? user = cursor.FirstOrDefault(cancellationToken);
            if (user == null)
                _logger?.LogDebug("User with id '{id}' not found in database.", id);
            return user;
        }

        internal async Task<UserDPO?> GetByHandleAsync(string handle, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting User with handle '{handle}' from database.", handle);
            FilterDefinition<UserDPO> getByHandleFilter = new FilterDefinitionBuilder<UserDPO>()
                .Eq(dpo => dpo.Handle, handle);
            IAsyncCursor<UserDPO> cursor = await _userCollection.FindAsync(getByHandleFilter, options: null, cancellationToken);
            UserDPO? user = cursor.FirstOrDefault(cancellationToken);
            if(user == null)
                _logger?.LogDebug("No User with handle '{handle}' found in database.", handle);
            return user;
        }

        public async Task<bool> RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Removing User with id '{id}' from database.", id);
            FilterDefinition<UserDPO> deleteByIdFilter = new FilterDefinitionBuilder<UserDPO>()
                .Eq(user => user.Id, id);
            DeleteResult deleteResult = await _userCollection.DeleteOneAsync(deleteByIdFilter, options: null, cancellationToken);
            if (deleteResult.DeletedCount == 0)
            {
                _logger?.LogDebug("User with id '{id}' not found in database.", id);
                return false;
            }
            return true;
        }
    }
}
