using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;
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
using WholeSaler.Entity.Entities.Products;
using StackExchange.Redis;
using WholeSaler.Business.Redis_Cache.RedisConfig;
using System.Text.Json;
using MongoDB.Bson.IO;
using System.Text.Json.Serialization;
using WholeSaler.Entity.Entities.Products.Features;
using System.Reflection;
using Newtonsoft.Json;


namespace WholeSaler.Business.ConcreteServices
{
    public class ProductServiceWithRedis : MongoWithRedisRepo<Product>, IProductServiceWithRedis
    {

        private readonly BaseMongoDBRepo<Product> _mongoDB;
        private readonly string entitykey = typeof(Product).Name;

        public ProductServiceWithRedis(IRedis_Cache<Product> redis, BaseMongoDBRepo<Product> mongoDB) : base(redis, mongoDB)
        {

            _mongoDB = mongoDB;


        }
        public async Task<List<Product>> GetTheStoreWithProducts(string storeId)
        {
            var collection = _mongoDB.GetCollection();

            var filter = Builders<Product>.Filter.Eq(x => x.Store.StoreId, storeId);
            var projection = Builders<Product>.Projection
        .Include(x => x.Id)
        .Include(x => x.Name)
        .Include(x => x.Category)
        .Include(x => x.UnitPrice)
        .Include(x => x.Color)
        .Include(x => x.SumOfSales)
        .Include(x => x.Brand)
        .Include(x => x.Stock)
        .Include(x => x.Quantity)
        .Include(x => x.Status)
        .Include(x => x.Images)
        .Include(x => x.Store);
            try
            {
                //var productsInStore = await collection.Find(filter).ToListAsync();
                var productsInStore = await collection.Find(filter).Project<Product>(projection).ToListAsync();
                return productsInStore;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;




        }
        public async Task<Product> GetProductForReview(string id)
        {
            var collection = _mongoDB.GetCollection();

            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var projection = Builders<Product>.Projection
        .Include(x => x.Id)
        .Include(x => x.Name)
        .Include(x => x.Category)
        .Include(x => x.UnitPrice)
        .Include(x => x.Color)
        .Include(x => x.SumOfSales)
        .Include(x => x.Brand)
        .Include(x => x.Stock)
        .Include(x => x.Quantity)
        .Include(x => x.Status)
        .Include(x => x.Images)
        .Include(x => x.Store);
            try
            {
                //var productsInStore = await collection.Find(filter).ToListAsync();
                var product = await collection.Find(filter).Project<Product>(projection).FirstOrDefaultAsync();
                return product;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;



        }



        public async Task<Product> GetProduct(string id)
        {
            var redisDb = RedisDB();
            var entity = await redisDb.HashGetAsync(entitykey, id);

            var product = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseProduct>(entity);

            switch (product.Category.SubCategory.Name)
            {
                case "Laptop":
                    var laptop = Newtonsoft.Json.JsonConvert.DeserializeObject<Laptop>(entity);
                    return laptop;
                case "Television":
                    var tv = Newtonsoft.Json.JsonConvert.DeserializeObject<Television>(entity);
                    return tv;
                default:
                    return null;
            }





        }

        public override Task<bool> Delete(string id)
        {
            return base.Delete(id);
        }

        public override async Task<IEnumerable<Product>> GetAll()
        {
            try
            {

                var products = new List<Product>();
                var redisDb = RedisDB();

                var cacheEntity = await redisDb.HashGetAllAsync(entitykey);

                var entityList = cacheEntity.Select(item => Newtonsoft.Json.JsonConvert.DeserializeObject<BaseProduct>(item.Value)).ToList();




                foreach (var document in entityList)
                {
                    if (document == null)
                    {
                        continue;
                    }

                    string type = document.Category.SubCategory.Name;  // Assuming 'Type' is a property of 'BaseProduct' or its derived classes

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(document); // Convert document to JSON string

                    switch (type)
                    {
                        case "Laptop":
                            var laptop = Newtonsoft.Json.JsonConvert.DeserializeObject<Laptop>(jsonString);
                            products.Add(laptop);
                            break;
                        case "Television":
                            var tv = Newtonsoft.Json.JsonConvert.DeserializeObject<Television>(jsonString);
                            products.Add(tv);
                            break;
                        default:
                            var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(jsonString);
                            products.Add(product);
                            break;
                    }
                }

                if (products.Count > 0)
                {
                    return products;
                }

                var collection = MongoDB();
                var documents = await collection.Find(new BsonDocument()).ToListAsync();
                //var products = new List<Product>();

                // Deserialize each document based on the "_t" field
                foreach (var document in documents)
                {
                    var type = document["_t"].AsString;

                    switch (type)
                    {
                        case "Laptop":
                            var laptop = BsonSerializer.Deserialize<Laptop>(document);
                            products.Add(laptop);
                            break;
                        case "Television":
                            var tv = BsonSerializer.Deserialize<Television>(document);

                            products.Add(tv);
                            break;
                        default:
                            products.Add(BsonSerializer.Deserialize<Product>(document));
                            break;
                    }
                }





                return products;
            }
            catch
            {
                throw;
            }
        }

        public override async Task<Product> GetById(string id)
        {
            var redisDb = RedisDB();
            var entity = await redisDb.HashGetAsync(entitykey, id);
            var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(entity);

            var subcategoryName = product.Category.SubCategory.Name;

           
            var typeMap = new Dictionary<string, Type>
          {
        { "Laptop", typeof(Laptop) },
        { "Television", typeof(Television) }
     
           };

            if (typeMap.TryGetValue(subcategoryName, out var targetType))
            {
                return (Product)Newtonsoft.Json.JsonConvert.DeserializeObject(entity, targetType);
            }

            return null;

        }

        public override Task<Product> Create(Product entity)
        {

            return base.Create(entity);
        }

        public override async Task<Product> Update(string updatedId, Product entity)
        {


            try
            {
                var existingEntity = await GetExistingEntityFromMongo(updatedId);
                var prd = UpdatedType(existingEntity["Type"].ToString(), existingEntity);

                if (prd != null)
                {
                    UpdateEntityProperties(prd, entity);
                    await UpdateMongoDBEntity(updatedId, prd);
                    await UpdateRedisEntity(prd);
                }

                return prd;
            }
            catch (Exception ex)
            {
                // Log the exception details here
                throw new Exception($"An error occurred while updating the product: {ex.Message}");
            }
        }



        public override Task<bool> ChangeStatusOfEntity(string id, int statusCode)
        {
            return base.ChangeStatusOfEntity(id, statusCode);
        }



        private IDatabase RedisDB()
        {
            var cm = ConnectionMultiplexer.Connect("localhost:6379");
            var redisDb = cm.GetDatabase(RedisDatabase.WholeSalerTestDB);
            return redisDb;
        }
        private IMongoCollection<BsonDocument> MongoDB()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("WholeSalerDB");
            var collection = database.GetCollection<BsonDocument>("Product");
            return collection;
        }
        private Product UpdatedType(string type, BsonDocument entity)
        {
            Product prd;
            switch (type)
            {
                case "Television":
                    prd = BsonSerializer.Deserialize<Television>(entity);
                    return prd;
                case "Laptop":
                    prd = BsonSerializer.Deserialize<Laptop>(entity);
                    return prd;
                default:
                    break;
            }
            return null;
        }

        private async Task<BsonDocument> GetExistingEntityFromMongo(string updatedId)
        {
            var collection = MongoDB();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(updatedId));
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
        private async Task UpdateMongoDBEntity(string updatedId, Product prd)
        {
            var collection = MongoDB();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(updatedId));
            await collection.ReplaceOneAsync(filter, prd.ToBsonDocument());
        }

        private async Task UpdateRedisEntity(Product prd)
        {
            var redis = RedisDB();
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            var serializedEntity = Newtonsoft.Json.JsonConvert.SerializeObject(prd, settings);
            await redis.HashSetAsync(entitykey, prd.Id, serializedEntity);
        }
        private void UpdateEntityProperties(Product prd, Product entity)
        {
            Type type = prd.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                var newValue = property.GetValue(entity);
                if (newValue != null && !(newValue is string strValue && string.IsNullOrEmpty(strValue)))
                {
                    property.SetValue(prd, newValue);
                }
            }
        }

    }
}
