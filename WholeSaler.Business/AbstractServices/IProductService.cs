using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.AbstractServices
{
    public interface IProductService
    {
        Task<Product> GetById(string id);
        Task<IEnumerable<Product>> GetAll();
        Task<Product> Create(Product entity);
        Task<Product> Update(string updatedId, Product entity);
        Task<bool> Delete(string id);
        Task<bool> ChangeStatusOfEntity(string id, int statusCode);
    }
}
