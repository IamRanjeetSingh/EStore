using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Infrastructure.Models.UserModel.MongoDb;
using UserService.Infrastructure.Models.UserModel.MongoDb.DataPersistenceObjects;

namespace UserService.Infrastructure.Utils
{
    internal sealed class MongoDbClientFactory : IMongoDbClientFactory
    {
        public IMongoCollection<UserDPO> GetUserCollection(UserMongoDbDao.Options options)
        {
            IMongoClient client = new MongoClient(options.ConnectionString);
            IMongoDatabase database = client.GetDatabase(options.DatabaseName);
            IMongoCollection<UserDPO> userCollection = database.GetCollection<UserDPO>(options.UserCollectionName);
            return userCollection;
        }
    }
}
