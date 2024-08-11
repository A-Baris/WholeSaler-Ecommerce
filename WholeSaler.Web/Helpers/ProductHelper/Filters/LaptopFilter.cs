using Newtonsoft.Json;
using WholeSaler.Web.Helpers.ProductHelper.Filters.BaseFilter;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Helpers.ProductHelper.Filters
{
    public class LaptopFilter : BaseProductFilter
    {
        public override List<ProductForCartVM> FilterProduct(ProductGeneralVm data, ProductFilterVm filterVm)
        {
            var prdFilter = filterVm.Laptop;
            var laptopFilter = data.Laptops
 .Where(x => (prdFilter.Brand == null || x.Brand == prdFilter.Brand) 
             && (prdFilter.RAM==null|| x.RAM <= prdFilter.RAM))
 .ToList();
            var prdJson = JsonConvert.SerializeObject(laptopFilter);
            var cartData = JsonConvert.DeserializeObject<List<ProductForCartVM>>(prdJson);
            return cartData;
        }
    }
}
