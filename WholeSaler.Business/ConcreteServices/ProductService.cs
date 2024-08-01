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
    public class ProductService : MongoWithRedisRepo<Product>, IProductService
    {
        public ProductService(IRedis_Cache<Product> redis, BaseMongoDBRepo<Product> mongoDB) : base(redis, mongoDB)
        {
        }

        public override Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            return base.ChangeStatusOfEntity(id, statusCode);
        }

        public override Task<Product> Create(Product entity)
        {
            return base.Create(entity);
        }

        public override Task<bool> Delete(string id)
        {
            return base.Delete(id);
        }

    

        public override Task<IEnumerable<Product>> GetAll()
        {
            return base.GetAll();
        }

        public override Task<Product> GetById(string id)
        {
            return base.GetById(id);
        }

   


        public override Task<Product> Update(string updatedId, Product newEntity)
        {
            return base.Update(updatedId, newEntity);
        }
    }
}
