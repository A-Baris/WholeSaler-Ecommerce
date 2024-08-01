using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Models.ViewModels.Product.Services
{
    public interface IProductFilterService
    {
        List<ProductForCartVM> GetFilteredProducts(BaseProductFilterVM filter, List<ProductForCartVM> data);
    }
}
