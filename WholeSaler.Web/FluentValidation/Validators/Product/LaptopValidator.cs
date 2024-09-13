using FluentValidation;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;

namespace WholeSaler.Web.FluentValidation.Validators.Product
{
    public class LaptopValidator:AbstractValidator<Laptop>
    {
        public LaptopValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty or null").NotNull().WithMessage("Name can not be empty or null").MinimumLength(3).WithMessage("Minumum 3 characters");
            RuleFor(x => x.Color).NotEmpty().WithMessage($"Color can not be empty or null").NotNull().WithMessage("Color can not be empty or null").MinimumLength(3).WithMessage("Minumum 3 characters");
            RuleFor(x => x.Brand).NotEmpty().WithMessage($"Brand can not be empty or null").NotNull().WithMessage("Brand can not be empty or null").MinimumLength(3).WithMessage("Minumum 2 characters");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage($"Unit Price can not be empty or null").NotNull().WithMessage("Unit Price can not be empty or null").GreaterThanOrEqualTo(1).WithMessage("Unit Price can not be less than 1 ");
            RuleFor(x => x.Stock).NotEmpty().WithMessage($"Stock can not be empty or null").NotNull().WithMessage("Stock can not be empty or null").GreaterThanOrEqualTo(5).WithMessage("Stock can not be less than 5");
            RuleFor(x => x.Description).NotEmpty().WithMessage($"Description can not be empty or null").NotNull().WithMessage("Description can not be empty or null").MinimumLength(10).WithMessage("Minimum 10 characters");

        }
    }
}
