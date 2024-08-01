using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Category
{
    public class CategoryVM
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<SubCategoryVM> SubCategories { get; set; }
    }
}
