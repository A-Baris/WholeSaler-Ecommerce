using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Helpers.ProductHelper
{
    public interface IProductFilterService
    {
        (List<ProductForCartVM>,Dictionary<string,object>) GetFilteredProducts(ProductGeneralVm data,ProductFilterVm productFilterVm);
       
    }
}
