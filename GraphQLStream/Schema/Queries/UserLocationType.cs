using System;

namespace GraphQLStream.Schema.Queries
{
    public class UserLocationType
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }

    }
}