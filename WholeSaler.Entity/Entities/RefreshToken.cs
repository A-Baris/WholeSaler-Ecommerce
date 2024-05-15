using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities
{
    public class RefreshToken:BaseEntity
    {
   
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expires { get; set; }
    }
}
