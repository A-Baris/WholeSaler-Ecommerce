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
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Business.ConcreteServices
{
    public class StoreServiceWithRedis : MongoWithRedisRepo<Store>, IStoreServiceWithRedis
    {
       
    
        public StoreServiceWithRedis(IRedis_Cache<Store> redis, BaseMongoDBRepo<Store> mongoDB) : base(redis, mongoDB)
        {

            
        }

    }
}
