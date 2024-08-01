using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.AbstractRepo
{
    public abstract class BaseMongoDBRepo<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;


        protected BaseMongoDBRepo(IMongoClient mongoClient, IConfiguration config)
        {
            var databaseName = config["ConnectionStrings:DatabaseName"];
            var collectionName = typeof(T).Name;
            _collection = mongoClient.GetDatabase(databaseName).GetCollection<T>(collectionName);

        }

        public IMongoCollection<T> GetCollection()
        {
            return _collection;
        }

        public abstract Task<T> GetAsync(string id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<T> Create(T entity);
        public abstract Task<T> Update(T entity);
        public abstract Task<bool> Delete(string id);
        public abstract Task<bool> ChangeStatusOfEntity(string id, int statusCode);
    }
}
