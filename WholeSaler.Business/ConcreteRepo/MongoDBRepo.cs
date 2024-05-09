using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Business.ConcreteRepo
{
    public class MongoDBRepo<T> : IMongoDBRepo<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        public MongoDBRepo(IMongoClient mongoClient, IConfiguration config)
        {
            var databaseName = config["ConnectionStrings:DatabaseName"];
            var collectionName = typeof(T).Name;
            _collection = mongoClient.GetDatabase(databaseName).GetCollection<T>(collectionName);
        }
     
        public async Task<T> Create(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await _collection.FindOneAndDeleteAsync(filter);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
            return entities;
        }

        public async Task<T> GetAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var entity = await _collection.Find(filter).FirstOrDefaultAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            try
            {
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
                    Console.WriteLine($"updateError ${ex.Message}");
                }
                
            }
            else
            {
               
                throw new InvalidOperationException("Entity is not found.");
            }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return entity;
        }
        public async Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {

            var filter = Builders<T>.Filter.Eq("Id", id);
            var entity = await _collection.Find(filter).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Status = (BaseStatus)statusCode;
                var result = await _collection.ReplaceOneAsync(filter, entity);
                return result.IsAcknowledged == true ? true : false;

            }
            return false;
        }

        IMongoCollection<T> IMongoDBRepo<T>.GetCollection()
        {
           return _collection;
        }
    }
}
