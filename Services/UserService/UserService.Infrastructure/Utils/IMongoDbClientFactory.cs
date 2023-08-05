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
    internal interface IMongoDbClientFactory
    {
        public IMongoCollection<UserDPO> GetUserCollection(UserMongoDbDao.Options options); 
    }
}
