using FluentValidation;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;

namespace WholeSaler.Web.FluentValidation.Validators.Category
{
    public class CategoryCreate:AbstractValidator<CategoryVM>
    {
        public CategoryCreate()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name can not be empty or null").NotNull().WithMessage("Name can not be empty or null").MinimumLength(3).WithMessage("Minumum 3 characters");
           
        }
    }
}
