using Microsoft.Extensions.Configuration;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WholeSaler.Business.Logger;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;
using ZstdSharp;

namespace WholeSaler.Business.Redis.Concrete
{
    public class Redis_Cache<T> : IRedis_Cache<T> where T : BaseEntity
    {
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly string _entityKey;
        private readonly ILogger _logger;

        public Redis_Cache(IConfiguration config)
        {
            var url = config["ConnectionStrings:Redis"];
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            var redis = ConnectionMultiplexer.Connect($"{url},abortConnect=false");
            _database = redis.GetDatabase(1);
            _entityKey = typeof(T).Name;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                if (await _database.HashDeleteAsync(_entityKey, id))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"#Redis-Cache deleting entity with ID {id}.", ex);
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var cacheEntity = await _database.HashGetAllAsync(_entityKey);
                var entityList = cacheEntity.Select(item => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item.Value)).ToList();
                return entityList;
            }
            catch (Exception ex)
            {
                _logger.LogError("#Redis-Cache GetAll entities.", ex);
                throw;
            }
        }

        public async Task<T> GetById(string id)
        {
            try
            {
                if (_database.KeyExists(_entityKey))
                {
                    var entity = await _database.HashGetAsync(_entityKey, id);
                    return entity.HasValue ? Newtonsoft.Json.JsonConvert.DeserializeObject<T>(entity) : null;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"#Redis-Cache GetById ID {id}.", ex);
                throw;
            }
        }

        //public async Task<T> SetEntity(T entity)
        //{
        //    try
        //    {
        //        await _database.HashSetAsync(_entityKey, entity.Id, JsonSerializer.Serialize(entity));
        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("#Redis-Cache  SetEntity.", ex);
        //        throw;
        //    }
        //}
        public async Task<T> SetEntity(T entity)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };

                var serializedEntity = Newtonsoft.Json.JsonConvert.SerializeObject(entity, settings);
                await _database.HashSetAsync(_entityKey, entity.Id, serializedEntity);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("#Redis-Cache SetEntity.", ex);
                throw;
            }
        }

        public async Task<T> Update(string updatedId, T newEntity)
        {
            try
            {
                var updatedEntity = await _database.HashGetAsync(_entityKey, updatedId);
                await SetEntity(newEntity);
                return newEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"#Redis-Cache Update entity with [Id]: {updatedId}.", ex);
                throw;
            }
        }

        public async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            try
            {
                var redisData = await _database.HashGetAsync(_entityKey, id);
                var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(redisData);
                entity.Status = (BaseStatus)statusCode;
                await SetEntity(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"#Redis-Cache ChangeStatusOfEntity [Id]: {id}.", ex);
                return false;
            }
        }
    }
}
