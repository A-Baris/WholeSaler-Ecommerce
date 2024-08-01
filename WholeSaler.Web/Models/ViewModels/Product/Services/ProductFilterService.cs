//using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
//using WholeSaler.Web.Models.ViewModels.Product.Filters;
//using WholeSaler.Web.Models.ViewModels.Product.Filters.Electronics;
//using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

//namespace WholeSaler.Web.Models.ViewModels.Product.Services
//{
//    public class ProductFilterService : IProductFilterService
//    {
//        public List<ProductForCartVM> GetFilteredProducts(BaseProductFilterVM filter,List<ProductForCartVM> data)
//        {
//            if (filter is LaptopFilterVM laptopFilter)
//            {
//                return FilterLaptops(laptopFilter,data);
//            }
//            else if (filter is TelevisionFilterVM televisionFilter)
//            {
//                return FilterTelevision(televisionFilter, data);
//            }

//            return new List<ProductForCartVM>();
//        }

//        private List<ProductForCartVM> FilterTelevision(TelevisionFilterVM filter,List<ProductForCartVM> data)
//        {
//            return data.Where(p => p.Category.CategoryName.ToLower() == "television" &&
//                                         (string.IsNullOrEmpty(filter.Brand) || p.Brand == filter.Brand) &&
//                                         (string.IsNullOrEmpty(filter.Inc.ToString()) || p.Inc == filter.Inc) &&
//                                         (!filter.MinPrice.HasValue || p.UnitPrice >= filter.MinPrice) &&
//                                         (!filter.MaxPrice.HasValue || p.UnitPrice <= filter.MaxPrice)).ToList();
//        }

//        private List<ProductForCartVM> FilterLaptops(LaptopFilterVM filter, List<ProductForCartVM> data)
//        {
//            return data.Where(p => p.Category.SubCategory.Name.ToLower() == "computer" &&
//                                         (!string.IsNullOrEmpty(filter.Processor) || p.Processor == filter.Processor) &&
//                                         (!filter.RAM.HasValue || p.RAM == filter.RAM) &&
//                                         (!filter.MinPrice.HasValue || p.UnitPrice >= filter.MinPrice) &&
//                                         (!filter.MaxPrice.HasValue || p.UnitPrice <= filter.MaxPrice)).ToList();



//        }
//    }
//}
