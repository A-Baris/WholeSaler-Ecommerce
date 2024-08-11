

using Humanizer;
using Newtonsoft.Json.Linq;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Comprehensive;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.UpdateVMs;
using WholeSaler.Web.Helpers.ImageHelper;
using WholeSaler.Web.Helpers.PropertyCoppier;

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
                product.Laptop = jObject.ToObject<Laptop>();
              
            }
            if (productType == "Television")
            {
                product.Television = jObject.ToObject<WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted.Television>();
                
            }
            if (productType == "Perfume")
            {
                product.Perfume = jObject.ToObject<WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted.Perfume>();

            }


            return product;

        }
        public static ProductEditPostVM ChangeTypeForUpdatePost(ProductEditPostVM data, ProductImageUpdateVm imageUpdateVm)
        {


            if (data.Laptop != null)
            {
                if(data.Laptop.Images == null)
                {
                    data.Laptop.Images = new List<ProductImage>();
                    return data;
                }
                data.Laptop.Images = new List<ProductImage>();
                data.Laptop.Images = imageUpdateVm.Images;
                return data;
            }
            if (data.Television != null)
            {
                
                data.Television.Images = new List<ProductImage>();
                data.Television.Images = imageUpdateVm.Images;
                return data;
            }
            if (data.Perfume != null && data.Perfume.Images!=null)
            {
                data.Perfume.Images = new List<ProductImage>();
                data.Perfume.Images = imageUpdateVm.Images;
                return data;
            }

            return data;
        }
        public static ProductComprehensiveVM SetCreateProductType(ProductComprehensiveVM comprehensiveVM)
        {
            if (comprehensiveVM.Perfume != null)
            {
                 CopyProperty.CopyProperties(comprehensiveVM, comprehensiveVM.Perfume);
                return comprehensiveVM;
            }
            if (comprehensiveVM.Laptop != null)
            {
                CopyProperty.CopyProperties(comprehensiveVM, comprehensiveVM.Laptop);
                return comprehensiveVM;
            }
            return null;
        }
    }
}
