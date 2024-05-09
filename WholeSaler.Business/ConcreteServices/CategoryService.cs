using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteRepo;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.ConcreteServices
{
    public class CategoryService : MongoDBRepo<Category>, ICategoryService
    {
     

        public CategoryService(IMongoClient mongoClient, IConfiguration config) : base(mongoClient, config)
        {
           
        }
    }
}
