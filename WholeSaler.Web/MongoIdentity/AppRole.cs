using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace WholeSaler.Web.MongoIdentity
{
    [CollectionName("Roles")]
    public class AppRole:MongoIdentityRole<Guid>
    {
    }
}
