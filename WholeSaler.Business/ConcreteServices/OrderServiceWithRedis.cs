using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteRepo;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Business.ConcreteServices
{
    public class OrderServiceWithRedis : MongoDBWithRedis<Order>, IOrderServiceWithRedis
    {
        private readonly IMongoDBRepo<Order> _mongoDB;



        private readonly IMongoCollection<Store> _storeCollection;
        public OrderServiceWithRedis(IRedis_Cache<Order> redis, IMongoDBRepo<Order> mongoDB) : base(redis, mongoDB)
        {
            _mongoDB = mongoDB;




        }
        public async Task<List<SalesReport>> GetSalesReport(string storeId, DateTime startDate,DateTime endDate)
        {
       
            var orderCollection = _mongoDB.GetCollection();
            var orders = await orderCollection.Find(o =>
           o.Products.Any(p => p.Store.StoreId == storeId) &&
           (o.CreatedDate.DayOfYear >= startDate.DayOfYear && o.CreatedDate.DayOfYear <= endDate.DayOfYear))
       .ToListAsync();

            var report = orders
                .SelectMany(o => o.Products
                    .Where(p => p.Store.StoreId == storeId)
                    .Select(p => new OrderProduct
                    {
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice,
                        Id = p.Id,
                        StoreId=p.Store.StoreId,
                        OrderDate = o.CreatedDate
                    }))
                   .GroupBy(op => op.OrderDate.Date) // Group by the Date part of OrderDate
        .Select(g => new SalesReport
        {
           TotalPrice=g.Sum(p=>(p.UnitPrice*p.Quantity)),
            Id = string.Join(", ", g.Select(op => op.Id)), // Concatenate the IDs
            StoreId = g.First().StoreId, // Use the StoreId from the first item in the group
            OrderDate = g.Key // The group key is the OrderDate
        })
        .ToList();

            return report;


        }

    }
}