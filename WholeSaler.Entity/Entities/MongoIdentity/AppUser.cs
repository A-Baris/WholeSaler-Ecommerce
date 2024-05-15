using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WholeSaler.Entity.Entities.MongoIdentity
{
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        public List<Adress> Address { get; set; }
        public string? StoreId { get; set; }
    }
}
