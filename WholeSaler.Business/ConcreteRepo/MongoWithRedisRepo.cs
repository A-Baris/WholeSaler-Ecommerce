﻿using StackExchange.Redis;
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
    public class MongoWithRedisRepo<T> : BaseMongoWithRedisRepo<T> where T : BaseEntity
    {
        private readonly IRedis_Cache<T> _redis;
        private readonly BaseMongoDBRepo<T> _mongoDB;

        public MongoWithRedisRepo(IRedis_Cache<T> redis,BaseMongoDBRepo<T> mongoDB)
        {
            _redis = redis;
            _mongoDB = mongoDB;
        }

        public override async Task<bool> Delete(string id)
        {

            bool mongoResult = await _mongoDB.Delete(id);
            bool redisResult = await _redis.Delete(id);
            if (mongoResult == true && redisResult == true)
            {
                return true;
            }
            return false;




        }

        public override async Task<IEnumerable<T>> GetAll()
        {

            var cacheEntities = await _redis.GetAll();
            if (cacheEntities != null && cacheEntities.Any())
            {
                return cacheEntities;
            }
            else
            {
                return await _mongoDB.GetAllAsync();
            }




        }

        public override async Task<T> GetById(string id)
        {

            var redisEntity = await _redis.GetById(id);
            if (redisEntity != null)
            {
                return redisEntity;
            }
            if (redisEntity == null)
            {
                var mongoDbEntity = await _mongoDB.GetAsync(id);
                return mongoDbEntity;
            }
            return null;




        }

        public override async Task<T> Create(T entity)
        {
            var mongoDbResult = await _mongoDB.Create(entity);
            var redisResult = await _redis.SetEntity(entity);
            if (mongoDbResult != null && redisResult != null)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }

        public override async Task<T> Update(string updatedId, T newEntity)
        {
            var mongoDbResult = await _mongoDB.Update(newEntity);
            var redisResult = await _redis.Update(mongoDbResult.Id, mongoDbResult);
            if (mongoDbResult != null && redisResult != null)
            {
                return newEntity;
            }
            else
            {
                return null;
            }
        }
        public override async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            var mongoDbResult = await _mongoDB.ChangeStatusOfEntity(id, statusCode);
            var redisResult = await _redis.ChangeStatusOfEntity(id, statusCode);
            if (mongoDbResult && redisResult)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

     
    }
}
