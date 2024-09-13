using FluentValidation;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;

namespace WholeSaler.Web.FluentValidation.Validators.Product
{
    public class ProductEditValidator: AbstractValidator<ProductEditVM>
    {
        public ProductEditValidator()
        {
            When(comp => comp.Laptop != null, () =>
            {
                RuleFor(comp => comp.Laptop).SetValidator(new LaptopValidator());
            });
            When(comp => comp.Television != null, () =>
            {
                RuleFor(comp => comp.Television).SetValidator(new TelevisionValidator());
            });
            When(comp => comp.Perfume != null, () =>
            {
                RuleFor(comp => comp.Perfume).SetValidator(new PerfumeValidator());
            });
        }
    }
}
