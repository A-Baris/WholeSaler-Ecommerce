
using Newtonsoft.Json;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Helpers.ProductHelper.Filters;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Helpers.ProductHelper
{
    public class ProductFilterService : IProductFilterService
    {
        public (List<ProductForCartVM>, Dictionary<string, object>) GetFilteredProducts(ProductGeneralVm data, ProductFilterVm productFilterVm)
        {
          
            var cartData = new List<ProductForCartVM>();
            var filterInfo = new Dictionary<string, object>();
            if (productFilterVm.Television != null)
            {
                var tvFilter = new TelevisionFilter();
                filterInfo = productFilterVm.Television.GetNonNullProperties();
                cartData = tvFilter.FilterProduct(data, productFilterVm);
               
             
            }
            if (productFilterVm.Laptop != null)
            {
                var laptopFilter = new LaptopFilter();
              filterInfo = productFilterVm.Laptop.GetNonNullProperties();
                cartData = laptopFilter.FilterProduct(data, productFilterVm);
               
            }
            if (productFilterVm.Perfume != null) 
            {
                filterInfo = productFilterVm.Perfume.GetNonNullProperties();
                var prdFilter = productFilterVm.Perfume;
                var perfumeFilter = data.Perfumes.Where(x => (prdFilter.Brand == null || x.Brand == prdFilter.Brand)).ToList();
                var prdJson = JsonConvert.SerializeObject(perfumeFilter);
                cartData = JsonConvert.DeserializeObject<List<ProductForCartVM>>(prdJson);
            }
            return (cartData,filterInfo);
        }

       
    }
}
