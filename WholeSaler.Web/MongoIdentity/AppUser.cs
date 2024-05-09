using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using WholeSaler.Web.Models.ViewModels.UserVM;

namespace WholeSaler.Web.MongoIdentity
{
    [CollectionName("Users")]
    public class AppUser:MongoIdentityUser<Guid>
    {
        public List<UserAdressVM> Address { get; set; }
        public string? StoreId { get; set; }

    }
}
