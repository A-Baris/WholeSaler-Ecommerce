using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Business.Redis.Concrete
{
    public class Redis_Cache<T> : IRedis_Cache<T> where T : BaseEntity
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly string _entityKey;
        public Redis_Cache(int dbNo, string entityKey, string url)
        {
            _redis = ConnectionMultiplexer.Connect(url);
            _database = _redis.GetDatabase(dbNo);
            _entityKey = entityKey;

        }
        public async Task<bool> Delete(string id)
        {
            if(await _database.HashDeleteAsync(_entityKey, id))
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var cacheEntity = await _database.HashGetAllAsync(_entityKey);
            var entityList = cacheEntity.Select(item => JsonSerializer.Deserialize<T>(item.Value)).ToList();
            return entityList;
        }

        public async Task<T> GetById(string id)
        {
            if (_database.KeyExists(_entityKey))
            {
                var entity = await _database.HashGetAsync(_entityKey, id);
                return entity.HasValue ? JsonSerializer.Deserialize<T>(entity) : null;
            }
            return null;
        }

        public async Task<T> SetEntity(T entity)
        {
            await _database.HashSetAsync(_entityKey, entity.Id, JsonSerializer.Serialize(entity));
            return entity;
        }

        public async Task<T> Update(string updatedId, T newEntity)
        {
            var updatedEntity = await _database.HashGetAsync(_entityKey, updatedId);

            await SetEntity(newEntity);
            return newEntity;
        }
        public async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            var redisData = await _database.HashGetAsync(_entityKey, id);
            var entity = JsonSerializer.Deserialize<T>(redisData);
            entity.Status = (BaseStatus)statusCode;
            await SetEntity(entity);
            return true;
           

        }
    }
}
