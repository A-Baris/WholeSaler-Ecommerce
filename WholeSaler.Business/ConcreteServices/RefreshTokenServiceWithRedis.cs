using AspNetCore.Identity.MongoDbCore.Models;
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
        private readonly IRedis_Cache<RefreshToken> _redis;
        private readonly IMongoDBRepo<RefreshToken> _mongoDB;

        public RefreshTokenServiceWithRedis(IRedis_Cache<RefreshToken> redis, IMongoDBRepo<RefreshToken> mongoDB) : base(redis, mongoDB)
        {
            _redis = redis;
            _mongoDB = mongoDB;
        }
        public async Task<RefreshToken> FindByToken(string token)
        {

            var refreshTokens = await _redis.GetAll();
            var refreshToken = refreshTokens.Where(x => x.Token == token && x.Status == Entity.Entities.Enums.BaseStatus.Active).FirstOrDefault();
            if (refreshToken != null)
            {
                return refreshToken;
            }
            else
            {
                var refreshTokenCollection = _mongoDB.GetCollection();
                var filter = Builders<RefreshToken>.Filter.And(
         Builders<RefreshToken>.Filter.Eq(x => x.Token, token),
         Builders<RefreshToken>.Filter.Eq(x => x.Status, Entity.Entities.Enums.BaseStatus.Active)
     );


                var result = refreshTokenCollection.Find(filter).FirstOrDefault();
                return result;
            }
           


        }
        public async Task<RefreshToken> FindByUserId(string userId)
        {
            var refreshTokens = await _redis.GetAll();
            var refreshToken = refreshTokens.Where(x => x.UserId == userId && x.Status == Entity.Entities.Enums.BaseStatus.Active).FirstOrDefault();
            if (refreshToken != null) 
            {
                return refreshToken;
            }


            var refreshTokenCollection = _mongoDB.GetCollection();
            var filter = Builders<RefreshToken>.Filter.And(
                Builders<RefreshToken>.Filter.Eq(x => x.UserId, userId), 
                Builders<RefreshToken>.Filter.Eq(x => x.Status, Entity.Entities.Enums.BaseStatus.Active));


            var result = refreshTokenCollection.Find(filter).FirstOrDefault();
            return result;
        }
    }
}
