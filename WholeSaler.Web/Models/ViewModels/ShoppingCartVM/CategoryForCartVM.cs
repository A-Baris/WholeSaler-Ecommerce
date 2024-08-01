using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;

namespace WholeSaler.Web.Models.ViewModels.ShoppingCartVM
{
    public class CategoryForCartVM
    {
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public SubCategoryVM SubCategory { get; set; }
    }
}
