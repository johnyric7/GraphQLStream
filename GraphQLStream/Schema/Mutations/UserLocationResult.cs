using System;

namespace GraphQLStream.Schema.Mutations
{
    public class UserLocationResult
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
    }
}