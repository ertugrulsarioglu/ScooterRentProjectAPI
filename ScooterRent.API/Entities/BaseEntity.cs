﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScooterRent.API.Entities
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
