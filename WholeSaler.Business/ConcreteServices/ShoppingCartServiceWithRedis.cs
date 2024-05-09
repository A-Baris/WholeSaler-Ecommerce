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
    public class ShoppingCartServiceWithRedis : MongoDBWithRedis<ShoppingCart>, IShoppingCartServiceWithRedis
    {
        private readonly IMongoDBRepo<ShoppingCart> _mongoDB;
        private readonly IMongoCollection<ShoppingCart> _shoppingCartCollection;

        public ShoppingCartServiceWithRedis(IRedis_Cache<ShoppingCart> redis, IMongoDBRepo<ShoppingCart> mongoDB) : base(redis, mongoDB)
        {
            _mongoDB = mongoDB;
            _shoppingCartCollection = _mongoDB.GetCollection();
        }

        public async Task<bool> DeleteProductInCart(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {

                var filter = Builders<ShoppingCart>.Filter.ElemMatch(
                 x => x.Products,
                 Builders<Product>.Filter.Eq(p => p.Id, id));

                var update = Builders<ShoppingCart>.Update.PullFilter(
                 x => x.Products,
                 Builders<Product>.Filter.Eq(p => p.Id, id));


                var updateResult = await _shoppingCartCollection.UpdateOneAsync(filter, update);
                return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;



            }
            return false;
        }
        public async Task<ShoppingCart> GetCartForUser(string userId)
        {

            
            var result = await _shoppingCartCollection.Find(x=>x.UserId == userId && x.Status ==BaseStatus.Active).FirstOrDefaultAsync();
           
            return result != null ? result : null;

        }
        public async Task<Product> CheckProductInCart(string cartId, string productId)
        {
            var filter = Builders<ShoppingCart>.Filter.And(
        Builders<ShoppingCart>.Filter.Eq(x => x.Id, cartId),
        Builders<ShoppingCart>.Filter.ElemMatch(x => x.Products, Builders<Product>.Filter.Eq(p => p.Id, productId)));
            var cart = await _shoppingCartCollection.Find(filter).FirstOrDefaultAsync();
            if (cart != null)
            {
              
                var product = cart.Products.FirstOrDefault(p => p.Id == productId);
                return product;
            }
            else
            {
                return null; 
            }

        }

        public async Task<bool> SetPassiveStatusOfShoppingCart(string cartId)
        {
            var filter = Builders<ShoppingCart>.Filter.Eq(x => x.Id, cartId);
            var update = Builders<ShoppingCart>.Update.Set(x => x.Status, BaseStatus.Passive);
            var result = await _shoppingCartCollection.UpdateOneAsync(filter,update);
            return result != null  ? true:false;
        }
    }
}
