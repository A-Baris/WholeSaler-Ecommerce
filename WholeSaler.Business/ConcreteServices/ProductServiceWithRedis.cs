using MongoDB.Driver;
using SharpCompress.Common;
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
    public class ProductServiceWithRedis : MongoDBWithRedis<Product>, IProductServiceWithRedis
    {
        private readonly IMongoDBRepo<Product> _mongoDB;
     
        public ProductServiceWithRedis(IRedis_Cache<Product> redis, IMongoDBRepo<Product> mongoDB) : base(redis, mongoDB)
        {
            _mongoDB = mongoDB;
           
        }
        public async Task<List<Product>> GetTheStoreWithPRoducts(string storeId)
        {
            var collection = _mongoDB.GetCollection();

            var filter = Builders<Product>.Filter.Eq(x => x.Store.StoreId, storeId);
            try
            {
                var productsInStore = await collection.Find(filter).ToListAsync();
                return productsInStore;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
           

           
            
        }
       
    }
}
