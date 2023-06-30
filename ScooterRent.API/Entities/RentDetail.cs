using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ScooterRent.API.Entities
{
    public class RentDetail : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ScooterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public double StartLat { get; set; }
        public double StartLang { get; set; }
        public double EndLat { get; set; }
        public double EndLang { get; set; }
    }
}
