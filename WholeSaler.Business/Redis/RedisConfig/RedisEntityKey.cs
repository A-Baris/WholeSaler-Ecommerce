using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Business.Redis_Cache.RedisConfig
{
    public class RedisEntityKey
    {
        public const string UserKey = "User-Cache";
        public const string ProductKey = "Product-Cache";
        public const string CategoryKey = "Category-Cache";
        public const string SubCategoryKey = "SubCategory-Cache";
        public const string StoreKey = "Store-Cache";
        public const string ShoppingCartKey = "ShoppingCart-Cache";
        public const string OrderKey = "Order-Cache";
        public const string MessageKey = "Message-Cache";
        public const string RefreshTokenKey = "RefreshToken-Cache";
    }
}
