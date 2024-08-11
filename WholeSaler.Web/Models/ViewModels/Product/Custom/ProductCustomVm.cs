using System.Reflection;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Models.ViewModels.Product.Custom
{
    public class ProductCustomVm
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Stock { get; set; }
        public int SumOfSales { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public List<ProductImage>? Images { get; set; }
        public CategoryForCartVM? Category { get; set; }
        public StoreForCartVM? Store { get; set; }

        public Dictionary<string, object> GetNonNullProperties()
        {
            var nonNullProperties = new Dictionary<string, object>();

            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                var value = property.GetValue(this);
                if (value != null)
                {
                    if (value is int intValue && intValue > 0)
                    {
                        nonNullProperties.Add(property.Name, value);
                    }
                    else if (value is decimal decimalValue && decimalValue > 0)
                    {
                        nonNullProperties.Add(property.Name, value);
                    }
                    else if (!(value is int || value is decimal || value is double || value is float || value is long || value is short))
                    {

                        nonNullProperties.Add(property.Name, value);
                    }
                }
            }

            return nonNullProperties;
        }
    }
}
