using FluentValidation;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WholeSaler.Web.Models.ViewModels.UserVM;

namespace WholeSaler.Web.FluentValidation.Validators.User
{
    public class LoginValidator : AbstractValidator<LoginVM>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Email can not be null").EmailAddress().WithMessage("aaa@bbb is corect version");
            RuleFor(x => x.Password).NotNull().WithMessage("Password can not be null");
        }
    }
}
