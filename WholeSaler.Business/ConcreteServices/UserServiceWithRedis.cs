using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteRepo;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.ConcreteServices
{
    public class UserServiceWithRedis : MongoWithRedisRepo<User>, IUserServiceWithRedis
    {
        private readonly BaseMongoDBRepo<User> _mongoDB;

        public UserServiceWithRedis(IRedis_Cache<User> redis, BaseMongoDBRepo<User> mongoDB) : base(redis, mongoDB)
        {
            _mongoDB = mongoDB;
        }
        public async Task<User> AuthenticateUser(string username, string password)
        {
            var collection = _mongoDB.GetCollection();
            
            var filter = Builders<User>.Filter.Eq(u => u.Username, username) & Builders<User>.Filter.Eq(u => u.Password, password);
            try
            {

           
            var foundUser =  collection.Find(filter).FirstOrDefault();
                return foundUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

           
        }
      
    }
}
