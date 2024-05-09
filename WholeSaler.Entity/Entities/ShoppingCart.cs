using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities
{
    public class ShoppingCart:BaseEntity
    {
        public string UserId { get; set; }
        public List<Product>? Products { get; set; }


    }
}
