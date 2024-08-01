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
    public abstract class BaseMongoWithRedisRepo<T> where T : BaseEntity
    {
        
        public abstract Task<T> GetById(string id);
        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<T> Create(T entity);
        public abstract Task<T> Update(string updatedId, T entity);
        public abstract Task<bool> Delete(string id);
        public abstract Task<bool> ChangeStatusOfEntity(string id, int statusCode);
    }
}
