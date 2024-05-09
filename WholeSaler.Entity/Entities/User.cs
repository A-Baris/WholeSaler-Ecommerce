using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities
{
    public class User:BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string? Phone { get; set; }
        public Adress? Adress { get; set; }

        public string? StoreId { get; set; }
   
    }
}
