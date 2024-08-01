using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.Logger;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Business.ConcreteRepo
{
    public class MongoDBRepo_<T> : BaseMongoDBRepo<T> where T : BaseEntity
    {
        private readonly ILogger _logger;
        public MongoDBRepo_(IMongoClient mongoClient, IConfiguration config, ILogger logger) : base(mongoClient, config)
        {
            _logger = logger;
        }

        public override async Task<T> Create(T entity)
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

        public override async Task<bool> Delete(string id)
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
                _logger.LogError($"#MongoDBRepo while deleting entity [Id]: {id}", ex);
                throw;
            }
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError($"#MongoDBRepo while  GetAll entities", ex);
                throw;
            }
        }

        public override async Task<T> GetAsync(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                var entity = await _collection.Find(filter).FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"#MongoDBRepo Get by [Id]:{id} ", ex);
                throw;
            }
        }

        public override async Task<T> Update(T entity)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", entity.Id);
                var existingEntity = await _collection.Find(filter).FirstOrDefaultAsync();
                if (existingEntity != null)
                {
                    // Iterate through each property of the entity
                    foreach (PropertyInfo property in typeof(T).GetProperties())
                    {
                        // Get the value of the current property in the entity
                        var newValue = property.GetValue(entity);

                        // Check if the new value is not null or empty (for string properties)
                        if (newValue != null && !(newValue is string strValue && string.IsNullOrEmpty(strValue)))
                        {
                            // Set the value to the existing entity
                            property.SetValue(existingEntity, newValue);
                        }
                    }

                    // Replace the existing entity in the collection
                    await _collection.ReplaceOneAsync(filter, existingEntity);
                    return existingEntity;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return entity;
        }
        public override async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
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
                _logger.LogError($"An error occurred while changing status of entity", ex);
                return false;
            }
        }
    }
}
