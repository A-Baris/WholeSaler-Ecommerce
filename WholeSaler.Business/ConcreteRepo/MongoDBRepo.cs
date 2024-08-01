using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SharpCompress.Common;
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
using WholeSaler.Entity.Entities.Products;

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


        //public async Task<T> Update(T entity)
        //{
        //    try
        //    {
        //        var idProperty = typeof(T).GetProperty("Id");
        //        if (idProperty == null)
        //        {
        //            throw new InvalidOperationException("Entity does not have an Id property.");
        //        }

        //        var idValue = idProperty.GetValue(entity);
        //        if (idValue == null)
        //        {
        //            throw new InvalidOperationException("Entity Id value is null.");
        //        }

        //        var filter = Builders<T>.Filter.Eq("Id", idValue);
        //        var existingEntity = await _collection.Find(filter).FirstOrDefaultAsync();

        //        if (existingEntity != null)
        //        {
        //            // Use reflection to get the properties of the entity
        //            var entityType = typeof(T);
        //            var properties = entityType.GetProperties();

        //            // Create an UpdateDefinition builder
        //            var updateDefinitionBuilder = Builders<T>.Update;

        //            // Create an empty UpdateDefinition
        //            UpdateDefinition<T> updateDefinition = null;

        //            foreach (var property in properties)
        //            {
        //                var newValue = property.GetValue(entity);
        //                var oldValue = property.GetValue(existingEntity);

        //                // Update only if the new value is not null and different from the old value
        //                if (newValue != null && !newValue.Equals(oldValue))
        //                {
        //                    if (updateDefinition == null)
        //                    {
        //                        updateDefinition = updateDefinitionBuilder.Set(property.Name, newValue);
        //                    }
        //                    else
        //                    {
        //                        updateDefinition = updateDefinition.Set(property.Name, newValue);
        //                    }
        //                }
        //            }

        //            // Add the UpdatedDate field to the update
        //            if (updateDefinition != null)
        //            {
        //                updateDefinition = updateDefinition.Set("UpdatedDate", DateTime.Now);
        //            }
        //            else
        //            {
        //                updateDefinition = updateDefinitionBuilder.Set("UpdatedDate", DateTime.Now);
        //            }

        //            if (updateDefinition != null)
        //            {
        //                try
        //                {
        //                    await _collection.UpdateOneAsync(filter, updateDefinition);
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.LogError($"#MongoDBRepo while Update [Id]:{idValue}", ex);
        //                    throw;
        //                }
        //            }
        //            else
        //            {
        //                _logger.LogInformation($"No fields to update for entity with [Id]:{idValue}");
        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Entity is not found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"#MongoDBRepo while Update general", ex);
        //        throw;
        //    }
        //    return entity;
        //}

        public async Task<T> Update(T entity)
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
