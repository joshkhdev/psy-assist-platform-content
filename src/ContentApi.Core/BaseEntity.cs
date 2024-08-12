using MongoDB.Bson;

namespace ContentApi.Core
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; }
    }
}
