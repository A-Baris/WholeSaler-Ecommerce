using WholeSaler.Web.Models.ViewModels.Product.Filters.Electronics;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using WholeSaler.Web.Models.ViewModels.Product;

namespace WholeSaler.Web.Helpers.ProductHelper
{
    public class ProductFilterService
    {
        //public List<ProductForCartVM> GetFilteredProducts(ProductForFilter filter, List<ProductForCartVM> products)
        //{
        //    var activeFilter = filter.GetActiveFilter();

        //    if (activeFilter is TelevisionFilterVM tvFilter)
        //    {
        //        return products.Where(product =>
        //            product.Category.SubCategory.Name == "Television" &&
        //            (!tvFilter.MinPrice.HasValue || product.UnitPrice >= tvFilter.MinPrice.Value) &&
        //            (!tvFilter.MaxPrice.HasValue || product.UnitPrice <= tvFilter.MaxPrice.Value) &&
        //            (string.IsNullOrEmpty(tvFilter.Brand) || product.Brand == tvFilter.Brand) &&
        //            (string.IsNullOrEmpty(tvFilter.Color) || product.Color == tvFilter.Color) &&
        //            (string.IsNullOrEmpty(tvFilter.ScreenSize) || product.ScreenSize == tvFilter.ScreenSize) &&
        //            (string.IsNullOrEmpty(tvFilter.Resolution) || product.Resolution == tvFilter.Resolution)
        //        ).ToList();
        //    }
        //    else if (activeFilter is LaptopFilterVM laptopFilter)
        //    {
        //        return products.Where(product =>
        //            product.Category.SubCategory.Name == "Laptop" &&
        //            (!laptopFilter.MinPrice.HasValue || product.UnitPrice >= laptopFilter.MinPrice.Value) &&
        //            (!laptopFilter.MaxPrice.HasValue || product.UnitPrice <= laptopFilter.MaxPrice.Value) &&
        //            (string.IsNullOrEmpty(laptopFilter.Brand) || product.Brand == laptopFilter.Brand) &&
        //            (string.IsNullOrEmpty(laptopFilter.Color) || product.Color == laptopFilter.Color) &&
        //            (!laptopFilter.RAM.HasValue || product.RAM == laptopFilter.RAM.Value) &&
        //            (string.IsNullOrEmpty(laptopFilter.Processor) || product.Processor == laptopFilter.Processor)
        //        ).ToList();
        //    }
        //    else if (activeFilter is BaseProductFilterVM baseFilter)
        //    {
        //        return products.Where(product =>
        //            (!baseFilter.MinPrice.HasValue || product.UnitPrice >= baseFilter.MinPrice.Value) &&
        //            (!baseFilter.MaxPrice.HasValue || product.UnitPrice <= baseFilter.MaxPrice.Value) &&
        //            (string.IsNullOrEmpty(baseFilter.Brand) || product.Brand == baseFilter.Brand) &&
        //            (string.IsNullOrEmpty(baseFilter.Color) || product.Color == baseFilter.Color)
        //        ).ToList();
        //    }

        //    return products;
        //}
    }
}
