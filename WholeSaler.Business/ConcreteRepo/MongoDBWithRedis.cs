using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.ConcreteRepo
{
    public class MongoDBWithRedis<T> : IMongoDBWithRedis<T> where T : BaseEntity
    {
        private readonly IRedis_Cache<T> _redis;
        private readonly IMongoDBRepo<T> _mongoDB;

        public MongoDBWithRedis(IRedis_Cache<T> redis,IMongoDBRepo<T> mongoDB)
        {
            _redis = redis;
            _mongoDB = mongoDB;
        }
        public async Task<bool> Delete(string id)
        {
            await _mongoDB.Delete(id);
            await _redis.Delete(id);
            return true;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var cacheEntities = await _redis.GetAll();
                if (cacheEntities != null && cacheEntities.Any())
                {
                    if (cacheEntities.Any())
                    {
                        return cacheEntities;
                    }
                }
                return await _mongoDB.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }

        public async Task<T> GetById(string id)
        {
           var entity = await _redis.GetById(id);
            return entity !=null ? entity : await _mongoDB.GetAsync(id);
        }

        public async Task<T> Create(T entity)
        {
            await _mongoDB.Create(entity);
            await _redis.SetEntity(entity);
            return entity;
        }

        public async Task<T> Update(string updatedId, T newEntity)
        {
            await _mongoDB.Update(newEntity);
            await _redis.Update(updatedId, newEntity);
            return newEntity;
        }
        public async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            await _mongoDB.ChangeStatusOfEntity(id, statusCode);
            await _redis.ChangeStatusOfEntity(id, statusCode);
            return true;
        }
    }
}
