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
    public class StoreServiceWithRedis : MongoDBWithRedis<Store>, IStoreServiceWithRedis
    {
        public StoreServiceWithRedis(IRedis_Cache<Store> redis, IMongoDBRepo<Store> mongoDB) : base(redis, mongoDB)
        {
        }
    }
}
