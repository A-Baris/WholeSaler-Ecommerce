
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteRepo;
using WholeSaler.Business.ConcreteServices;
using WholeSaler.Business.Logger;
using WholeSaler.Business.Redis.Concrete;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Business.Redis_Cache.RedisConfig;
using WholeSaler.Business.TokenServices.Abstract;
using WholeSaler.Business.TokenServices.Concrete;
using WholeSaler.Entity.Entities;
using ILogger = WholeSaler.Business.Logger.ILogger;

namespace WholeSaler.IOC.Container
{
    public class ConfigServices
    {
        public static void ServiceConfiguration(IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<ILogger, MicrosoftLogger>();

            //services.AddTransient(typeof(IMongoDBRepo<>), typeof(MongoDBRepo<>));
            //services.AddSingleton<IMongoClient>(sp =>
            //{
            //    var config = sp.GetRequiredService<IConfiguration>();
            //    var connectionString =config.GetConnectionString("MongoDB");
            //    var databaseName = config["ConnectionStrings:DatabaseName"];
            //    return new MongoClient(connectionString);
            //});

            services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionStrings = configuration["ConnectionStrings:MongoDB"];
                return new MongoClient(connectionStrings);
            });
        


            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var redisConnectionString = configuration["ConnectionStrings:Redis"];
                return ConnectionMultiplexer.Connect(redisConnectionString);

            });
            //abstractmongo servisler kontrol et!!
            services.AddScoped(typeof(IRedis_Cache<>), typeof(Redis_Cache<>));
            services.AddScoped(typeof(BaseMongoDBRepo<>), typeof(MongoDBRepo_<>)); // Register MongoDBRepo_<> as the implementation of BaseMongoDBRepo<>
            services.AddScoped(typeof(MongoWithRedisRepo<>));
            services.AddScoped<IRefreshTokenServiceWithRedis, RefreshTokenServiceWithRedis>();
            services.AddScoped<ITokenService, TokenService>();


            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IProductServiceWithRedis, ProductServiceWithRedis>();
            services.AddScoped<ICategoryServiceWithRedis, CategoryServiceWithRedis>();
            services.AddScoped<IStoreServiceWithRedis, StoreServiceWithRedis>();
            services.AddScoped<IMessageServiceWithRedis, MessageServiceWithRedis>();
            services.AddScoped<IUserServiceWithRedis, UserServiceWithRedis>();
            services.AddScoped<IShoppingCartServiceWithRedis, ShoppingCartServiceWithRedis>();
            services.AddScoped<IOrderServiceWithRedis, OrderServiceWithRedis>();
        

            #region updatedservices

            ////ServicesWithRedis
            //services.AddScoped<ICategoryServiceWithRedis, CategoryServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.CategoryKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<Category>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<Category>>();
            //    return new CategoryServiceWithRedis(redisCache, repo);
            //});




            //services.AddScoped<IProductServiceWithRedis, ProductServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.ProductKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<Product>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<Product>>();
            //    return new ProductServiceWithRedis(redisCache, repo);
            //});


            //services.AddScoped<IProductTestService, ProductTestService>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.ProductKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<ProductTest>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<ProductTest>>();
            //    return new ProductTestService(redisCache, repo);
            //});




            //services.AddScoped<IStoreServiceWithRedis, StoreServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.StoreKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<Store>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<Store>>();
            //    return new StoreServiceWithRedis(redisCache, repo);
            //});

            //services.AddScoped<IUserServiceWithRedis, UserServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.UserKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<User>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<User>>();
            //    return new UserServiceWithRedis(redisCache, repo);
            //});

            //services.AddScoped<IShoppingCartServiceWithRedis, ShoppingCartServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.ShoppingCartKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<ShoppingCart>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<ShoppingCart>>();
            //    return new ShoppingCartServiceWithRedis(redisCache, repo);
            //});

            //services.AddScoped<IOrderServiceWithRedis, OrderServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.OrderKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<Order>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<Order>>();
            //    return new OrderServiceWithRedis(redisCache, repo);
            //});

            //services.AddScoped<IMessageServiceWithRedis, MessageServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.MessageKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<Message>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<Message>>();
            //    return new MessageServiceWithRedis(redisCache, repo);
            //});


            //services.AddScoped<IRefreshTokenServiceWithRedis, RefreshTokenServiceWithRedis>(sp =>
            //{
            //    var dbNo = RedisDatabase.WholeSalerTestDB;
            //    var entityKey = RedisEntityKey.RefreshTokenKey;
            //    var url = configuration["CacheOptions:Url"];
            //    var logger = sp.GetService<ILogger>();
            //    var redisCache = new Redis_Cache<RefreshToken>(dbNo, entityKey, url, logger);
            //    var repo = sp.GetService<IMongoDBRepo<RefreshToken>>();
            //    return new RefreshTokenServiceWithRedis(redisCache, repo);
            //}); 
            #endregion




        }
    }
}
