using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.AbstractRepo;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteRepo;
using WholeSaler.Business.Redis_Cache.Abstracts;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Business.ConcreteServices
{
    public class RefreshTokenServiceWithRedis : MongoDBWithRedis<RefreshToken>, IRefreshTokenServiceWithRedis
    {
        private readonly IMongoDBRepo<RefreshToken> _mongoDB;

        public RefreshTokenServiceWithRedis(IRedis_Cache<RefreshToken> redis, IMongoDBRepo<RefreshToken> mongoDB) : base(redis, mongoDB)
        {
           _mongoDB = mongoDB;
        }
        public async Task<RefreshToken> FindByToken(string token)
        {
           var refreshTokenCollection= _mongoDB.GetCollection();
           var filter = Builders<RefreshToken>.Filter.And(
     Builders<RefreshToken>.Filter.Eq(x => x.Token, token),
     Builders<RefreshToken>.Filter.Eq(x => x.Status, Entity.Entities.Enums.BaseStatus.Active)
 );

            
            var result = refreshTokenCollection.Find(filter).FirstOrDefault();
            return result;
        }
        public async Task<RefreshToken> FindByUserId(string userId)
        {
            var refreshTokenCollection = _mongoDB.GetCollection();
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.UserId, userId));


            var result = refreshTokenCollection.Find(filter).FirstOrDefault();
            return result;
        }
    }
}
