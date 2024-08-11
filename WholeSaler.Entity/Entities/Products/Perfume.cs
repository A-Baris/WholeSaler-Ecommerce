using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Products
{
    public class Perfume:Product
    {
        public string? PerfumeType { get; set; }
        public int? Volume { get; set; }
        public string? Smell { get; set; }
        public string? SmellType { get; set; }
      

    }
}
