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

namespace WholeSaler.Business.ConcreteServices
{
    public class MessageServiceWithRedis : MongoWithRedisRepo<Message>, IMessageServiceWithRedis
    {
        public MessageServiceWithRedis(IRedis_Cache<Message> redis, BaseMongoDBRepo<Message> mongoDB) : base(redis, mongoDB)
        {
        }
    }
}
