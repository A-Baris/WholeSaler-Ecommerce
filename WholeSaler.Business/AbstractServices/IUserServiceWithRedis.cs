using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.MongoIdentity;

namespace WholeSaler.Business.AbstractServices
{
    public interface IUserServiceWithRedis:IMongoDBWithRedis<User>
    {
        Task<User> AuthenticateUser(string username, string password);
    }
}
