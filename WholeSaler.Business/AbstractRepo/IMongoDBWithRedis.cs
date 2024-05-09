using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.AbstractRepo
{
    public interface IMongoDBWithRedis<T> where T : BaseEntity
    {
        Task<T> Create(T entity);
        Task<T> GetById(string id);

        Task<IEnumerable<T>> GetAll();
        Task<bool> Delete(string id);

        Task<T> Update(string updatedId, T newEntity);
        Task<bool> ChangeStatusOfEntity(string id, int statusCode);
    }
}
