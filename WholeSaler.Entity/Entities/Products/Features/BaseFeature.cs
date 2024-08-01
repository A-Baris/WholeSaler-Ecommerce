using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Products.Features
{
    public class BaseFeature
    {
        public LaptopFeature? Laptop { get; set; }
        public TelevisionFeature? Television { get; set; }
    }
}
