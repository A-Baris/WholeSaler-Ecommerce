using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using WholeSaler.Web.Models.ViewModels.UserVM;
using WholeSaler.Web.Models.Enums;

namespace WholeSaler.Web.MongoIdentity
{
    [CollectionName("User")]
    public class AppUser:MongoIdentityUser<Guid>
    {
        public List<UserAdressVM> Addresses { get; set; }
        public string? StoreId { get; set; }
        public string? ConnectionId { get; set; }
        public EntityStatus? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
