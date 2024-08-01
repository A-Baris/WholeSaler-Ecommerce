using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Products.Features
{
    public static class TestBsonObject
    {
        public static T ToObject<T>(this BsonValue value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return BsonSerializer.Deserialize<T>(value.AsBsonDocument);
        }
    }
}
