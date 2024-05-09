
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteRepo;
using WholeSaler.Business.ConcreteServices;
using WholeSaler.Business.Redis.Concrete;
using WholeSaler.Business.Redis_Cache.RedisConfig;
using WholeSaler.Entity.Entities;

namespace WholeSaler.IOC.Container
{
    public class ConfigServices
    {
        public static void ServiceConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IMongoDBRepo<>), typeof(MongoDBRepo<>));
            services.AddSingleton<IMongoClient>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var connectionString =config.GetConnectionString("MongoDB");
                var databaseName = config["ConnectionStrings:DatabaseName"];
                return new MongoClient(connectionString);
            });

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();



            //ServicesWithRedis
            services.AddScoped<ICategoryServiceWithRedis, CategoryServiceWithRedis>(sp =>
            {
                var dbNo = RedisDatabase.WholeSalerTestDB;
                var entityKey = RedisEntityKey.CategoryKey;
                var url = configuration["CacheOptions:Url"];
                var redisCache = new Redis_Cache<Category>(dbNo, entityKey, url);
                var repo = sp.GetService<IMongoDBRepo<Category>>();
                return new CategoryServiceWithRedis(redisCache,repo);
            });

            services.AddScoped<IProductServiceWithRedis, ProductServiceWithRedis>(sp =>
            {
                var dbNo = RedisDatabase.WholeSalerTestDB;
                var entityKey = RedisEntityKey.ProductKey;
                var url = configuration["CacheOptions:Url"];
                var redisCache = new Redis_Cache<Product>(dbNo, entityKey, url);
                var repo = sp.GetService<IMongoDBRepo<Product>>();
                return new ProductServiceWithRedis(redisCache, repo);
            });

            services.AddScoped<IStoreServiceWithRedis, StoreServiceWithRedis>(sp =>
            {
                var dbNo = RedisDatabase.WholeSalerTestDB;
                var entityKey = RedisEntityKey.StoreKey;
                var url = configuration["CacheOptions:Url"];
                var redisCache = new Redis_Cache<Store>(dbNo, entityKey, url);
                var repo = sp.GetService<IMongoDBRepo<Store>>();
                return new StoreServiceWithRedis(redisCache, repo);
            });

            services.AddScoped<IUserServiceWithRedis, UserServiceWithRedis>(sp =>
            {
                var dbNo = RedisDatabase.WholeSalerTestDB;
                var entityKey = RedisEntityKey.UserKey;
                var url = configuration["CacheOptions:Url"];
                var redisCache = new Redis_Cache<User>(dbNo, entityKey, url);
                var repo = sp.GetService<IMongoDBRepo<User>>();
                return new UserServiceWithRedis(redisCache, repo);
            });

            services.AddScoped<IShoppingCartServiceWithRedis, ShoppingCartServiceWithRedis>(sp =>
            {
                var dbNo = RedisDatabase.WholeSalerTestDB;
                var entityKey = RedisEntityKey.ShoppingCartKey;
                var url = configuration["CacheOptions:Url"];
                var redisCache = new Redis_Cache<ShoppingCart>(dbNo, entityKey, url);
                var repo = sp.GetService<IMongoDBRepo<ShoppingCart>>();
                return new ShoppingCartServiceWithRedis(redisCache, repo);
            });

            services.AddScoped<IOrderServiceWithRedis, OrderServiceWithRedis>(sp =>
            {
                var dbNo = RedisDatabase.WholeSalerTestDB;
                var entityKey = RedisEntityKey.OrderKey;
                var url = configuration["CacheOptions:Url"];
                var redisCache = new Redis_Cache<Order>(dbNo, entityKey, url);
                var repo = sp.GetService<IMongoDBRepo<Order>>();
                return new OrderServiceWithRedis(redisCache, repo);
            });
        }
    }
}
