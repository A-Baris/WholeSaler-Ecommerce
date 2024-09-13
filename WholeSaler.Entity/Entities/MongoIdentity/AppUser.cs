using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Enums;


namespace WholeSaler.Entity.Entities.MongoIdentity
{
    [CollectionName("User")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        public List<Adress>? Addresses { get; set; }
        public string? StoreId { get; set; }
        public string? ConnectionId { get; set; }
        public BaseStatus? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
