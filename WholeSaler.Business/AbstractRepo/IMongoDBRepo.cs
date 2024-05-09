using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.AbstractRepo
{
    public interface IMongoDBRepo<T> where T : BaseEntity
    {
        IMongoCollection<T> GetCollection();
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(string id);
        Task<bool> ChangeStatusOfEntity(string id, int statusCode);
    }
}
