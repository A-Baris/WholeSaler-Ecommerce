using Newtonsoft.Json;
using WholeSaler.Web.Helpers.ProductHelper.Filters.BaseFilter;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Custom;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WholeSaler.Web.Helpers.ProductHelper.Filters
{
    public class TelevisionFilter:BaseProductFilter
    {
        public override List<ProductForCartVM> FilterProduct(ProductGeneralVm data, ProductFilterVm filterVm)
        {
            var tvFilter = data.Televisions.Where(x => x.Brand == filterVm.Television.Brand).ToList();
            var prdJson = JsonConvert.SerializeObject(tvFilter);
            var cartData = JsonConvert.DeserializeObject<List<ProductForCartVM>>(prdJson);
            return cartData;
        }

       
    }
}
