using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Helpers.ProductHelper.Filters.BaseFilter
{
    public abstract class BaseProductFilter
    {
        public abstract List<ProductForCartVM> FilterProduct(ProductGeneralVm data, ProductFilterVm filterVm);
    }
}
