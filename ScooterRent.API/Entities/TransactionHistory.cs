using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ScooterRent.API.Enums;

namespace ScooterRent.API.Entities
{
    public class TransactionHistory : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
