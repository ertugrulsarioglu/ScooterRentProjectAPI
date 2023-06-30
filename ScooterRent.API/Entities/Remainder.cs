using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ScooterRent.API.Enums;

namespace ScooterRent.API.Entities
{
    public class Remainder : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public RemainderType RemainderType { get; set; }
    }
}
