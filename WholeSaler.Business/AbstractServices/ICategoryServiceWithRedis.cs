using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.AbstractServices
{
    public interface ICategoryServiceWithRedis : IMongoDBWithRedis<Category>
    {
       
    }
}
