using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.AbstractServices
{
    public interface IOrderServiceWithRedis:IMongoDBWithRedis<Order>
    {
        Task<List<SalesReport>> GetSalesReport(string storeId, DateTime startDate,DateTime endDate);


    }
}
