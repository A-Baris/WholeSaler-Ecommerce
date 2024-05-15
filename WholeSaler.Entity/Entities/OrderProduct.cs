using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities
{
    public class OrderProduct
    {
        public string Id { get; set; }
        public string StoreId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
