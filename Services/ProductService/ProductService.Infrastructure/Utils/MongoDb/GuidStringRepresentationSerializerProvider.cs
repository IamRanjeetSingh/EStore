using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace ProductService.Infrastructure.Utils.MongoDb
{
    public static class GuidStringRepresentationSerializerProvider
    {
        private static GuidSerializer? _instance = null;

        public static GuidSerializer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GuidSerializer(representation: BsonType.String);
                return _instance;
            }
        }
    }
}
