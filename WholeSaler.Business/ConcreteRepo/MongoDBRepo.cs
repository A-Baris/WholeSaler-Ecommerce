using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.Logger;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Business.ConcreteRepo
{
    public class MongoDBRepo<T> : IMongoDBRepo<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ILogger _logger;

        public MongoDBRepo(IMongoClient mongoClient, IConfiguration config,ILogger logger)
        {
            var databaseName = config["ConnectionStrings:DatabaseName"];
            var collectionName = typeof(T).Name;
            _collection = mongoClient.GetDatabase(databaseName).GetCollection<T>(collectionName);
          _logger = logger;
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                _logger.LogInformation("Entity created successfully.");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("#MongoDBRepo Create entity.", ex);
                throw;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id); 
                var result = await _collection.FindOneAndDeleteAsync(filter);
                if (result != null)
                {                
                    return true;
                }
                else
                {                  
                    return false;
                }
               
            }
            catch (Exception ex)
            {
               _logger.LogError($"#MongoDBRepo while deleting entity [Id]: {id}",ex);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
               _logger.LogError($"#MongoDBRepo while  GetAll entities",ex);
                throw;
            }
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                var entity = await _collection.Find(filter).FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
               _logger.LogError($"#MongoDBRepo Get by [Id]:{id} ",ex);
                throw;
            }
        }

        public async Task<T> Update(T entity)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", entity.Id);
                var existingEntity = await _collection.Find(filter).FirstOrDefaultAsync();

                if (existingEntity != null)
                {
                    entity.CreatedDate = existingEntity.CreatedDate;
                    entity.UpdatedDate = DateTime.Now;
                    try
                    {
                        await _collection.ReplaceOneAsync(filter, entity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"#MongoDBRepo while Update [Id]:{entity.Id}", ex);
                        throw;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Entity is not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"#MongoDBRepo while Update general",ex);
                throw;
            }
            return entity;
        }

        public async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                var entity = await _collection.Find(filter).FirstOrDefaultAsync();
                if (entity != null)
                {
                    entity.Status = (BaseStatus)statusCode;
                    var result = await _collection.ReplaceOneAsync(filter, entity);
                    return result.IsAcknowledged;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while changing status of entity",ex);
                return false;
            }
        }


        IMongoCollection<T> IMongoDBRepo<T>.GetCollection()
        {
            return _collection;
        }
    }
}
