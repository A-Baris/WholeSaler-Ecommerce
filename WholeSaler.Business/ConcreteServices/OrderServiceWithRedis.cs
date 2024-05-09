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
    public class OrderServiceWithRedis : MongoDBWithRedis<Order>, IOrderServiceWithRedis
    {
        private readonly IMongoDBRepo<Order> _mongoDB;
        private readonly IMongoCollection<Order> _orderCollection;
        public OrderServiceWithRedis(IRedis_Cache<Order> redis, IMongoDBRepo<Order> mongoDB) : base(redis, mongoDB)
        {
            _mongoDB = mongoDB;
            _orderCollection = _mongoDB.GetCollection();
        }

        //public async Task<List<Order>> GetAllUserOrders(string UserId)
        //{
        //    var filter = Builders<Order>.Filter.Eq(x => x.UserId, UserId);
        //    var result = await _orderCollection.Find(filter).ToListAsync();
        //    return result!=null?result:null;
            
        //}

    }

}
