namespace WholeSaler.Web.Areas.Admin.Models.ViewModels.Category
{
    public class CategoryCreateVM
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<SubCategoryVM> SubCategories { get; set; }

    }
}
