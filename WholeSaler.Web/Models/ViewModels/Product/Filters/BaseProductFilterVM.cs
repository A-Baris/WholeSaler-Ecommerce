using System.Reflection;

namespace WholeSaler.Web.Models.ViewModels.Product.Filters
{
    public class BaseProductFilterVM
    {
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Color { get; set; }

        public Dictionary<string, object> GetNonNullProperties()
        {
            var nonNullProperties = new Dictionary<string, object>();

            foreach (PropertyInfo property in typeof(BaseProductFilterVM).GetProperties())
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
