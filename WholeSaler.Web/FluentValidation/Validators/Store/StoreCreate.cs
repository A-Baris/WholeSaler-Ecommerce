using FluentValidation;
using FluentValidation.AspNetCore;
using System.Data;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Store;

namespace WholeSaler.Web.FluentValidation.Validators.Store
{
    public class StoreCreate: AbstractValidator<StoreCreateVM>
    {
        public StoreCreate()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name can not be empty").NotNull().WithMessage("Name can not be null");
            RuleFor(x=>x.Adress.Country).NotEmpty().WithMessage("Country can not be empty").NotNull().WithMessage("Country can not be null");
            RuleFor(x=>x.Adress.City).NotEmpty().WithMessage("City can not be empty").NotNull().WithMessage("City can not be null");
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Email can not be empty").NotNull().WithMessage("Email can not be null").EmailAddress().WithMessage("info@xxx is correct version");
            RuleFor(x=>x.Phone).NotEmpty().WithMessage("Phone can not be empty").NotNull().WithMessage("Phone can not be null").MinimumLength(10).WithMessage("05xx-xxx-xx-xx or 5xx-xxx-xx-xx").MaximumLength(11).WithMessage("05xx-xxx-xx-xx or 5xx-xxx-xx-xx");
            RuleFor(x => x.TaxNumber).NotEmpty().WithMessage("Tax No can not be empty").NotNull().WithMessage("Tax No can not be null");
        }
    }
}
