using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Models.UserModel.MongoDb.DataPersistenceObjects
{
    internal sealed class UserDPO
    {
        [BsonId]
        public string Id { get; set; }
        
        public string Handle { get; set; }

        public string Email { get; set; }

        public bool EmailIsVerified { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string CreationDate { get; set; }

        //TODO: find better way to handle this warning
#pragma warning disable CS8618 
        public UserDPO() { }
#pragma warning restore CS8618 
    }
}
