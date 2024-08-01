

using Humanizer;
using Newtonsoft.Json.Linq;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.UpdateVMs;
using WholeSaler.Web.Helpers.ImageHelper;

namespace wholesaler.web.helpers.producthelper
{
    public static class ProductHelper
    {
        public static ProductEditVM ChangeTypeForUpdateGet(string data)
        {

            var jObject = JObject.Parse(data);
            var productType = jObject["type"]?.ToString();

            var product = new ProductEditVM { Type = productType };

            if (productType == "Laptop")
            {
                product.Laptop = jObject.ToObject<LaptopUpdateVm>();
                // Fill in additional properties as needed
            }
            // Add other type checks and handling as necessary

            return product;

        }
        public static ProductEditPostVM ChangeTypeForUpdatePost(ProductEditPostVM data, ProductImageUpdateVm imageUpdateVm)
        {

            
                if (data.Laptop != null) 
                {
                    data.Laptop.Images= new List<ProductImage>();
                    data.Laptop.Images= imageUpdateVm.Images;
                return data;
                }
                
            return null;
        }
        
    }
}
