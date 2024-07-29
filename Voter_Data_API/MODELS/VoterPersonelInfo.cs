using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Voter_Data_API.MODELS
{
    public class VoterPersonelInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        [BsonIgnoreIfNull]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? Age { get; set; } 
    }
}
