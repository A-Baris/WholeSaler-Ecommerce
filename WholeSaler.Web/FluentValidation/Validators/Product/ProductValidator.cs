using FluentValidation;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Comprehensive;

namespace WholeSaler.Web.FluentValidation.Validators.Product
{
    public class ProductValidator: AbstractValidator<ProductComprehensiveVM>
    {
        public ProductValidator()
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
