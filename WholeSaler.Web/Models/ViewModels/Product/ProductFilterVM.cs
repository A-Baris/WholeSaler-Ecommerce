using System.Reflection;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;

namespace WholeSaler.Web.Models.ViewModels.Product
{
    public class ProductFilterVM
    {
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
   
        public string? Gender {  get; set; }

        public List<ProductVM> Products { get; set; }


        public Dictionary<string, object> GetNonNullProperties()
        {
            var nonNullProperties = new Dictionary<string, object>();

            foreach (PropertyInfo property in typeof(ProductFilterVM).GetProperties())
            {
                var value = property.GetValue(this);
                if (value != null)
                {
                    nonNullProperties[property.Name] = value;
                }
            }

            return nonNullProperties;
        }


    }
}
