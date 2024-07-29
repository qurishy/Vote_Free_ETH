using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Voter_Data_API.MODELS
{
    [CollectionName("Users")]
    public class ApplicationUsers : MongoIdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
    }
}
