using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infra.Utils.MongoDb
{
    internal static class GuidSerializerRegistrar
    {
        private static bool _isGuidSerializerRegistered = false;
        private static object _guidSerializerRegisterLock = new();

        internal static void Register()
        {
            if (_isGuidSerializerRegistered)
                return;

            lock (_guidSerializerRegisterLock)
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
                _isGuidSerializerRegistered = true;
            }
        }
    }
}
