using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GraphQLStream.DTO
{
    public class UserLocationDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }

        [BsonElement("createdTimestamp")]
        public DateTime CreatedTimestamp { get; set; }

        [BsonElement("updatedTimestamp")]
        public DateTime UpdatedTimestamp { get; set; }
    }
}