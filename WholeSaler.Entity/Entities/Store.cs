using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Entity.Entities
{
    public class Store:BaseEntity
    {
        public Store()
        {
            AdminConfirmation = 0;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Adress? Adress { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxNumber { get; set; }
        public AdminConfirmation? AdminConfirmation { get; set; }

        public string? UserId { get; set; }
   
    }
}
