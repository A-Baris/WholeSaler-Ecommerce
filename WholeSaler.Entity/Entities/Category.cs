using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Embeds.Category;
using WholeSaler.Entity.Entities.Enums;


namespace WholeSaler.Entity.Entities
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<SubCategory>? SubCategories { get; set; }

    }
}
