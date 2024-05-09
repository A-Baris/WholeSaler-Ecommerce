using FluentValidation;
using WholeSaler.Web.Models.ViewModels;

namespace WholeSaler.Web.FluentValidation.Validators.User
{
    public class RegisterValidator:AbstractValidator<RegisterVm>
    {
        public RegisterValidator()
        {
            RuleFor(x=>x.Username).NotNull()
                .WithMessage("Username can not be null").MinimumLength(5).WithMessage("Username can not be less than 5 character");
            
            RuleFor(x => x.Email).NotNull()
                .WithMessage("Email can not be null").EmailAddress().WithMessage("aaa@bbb is corect version");
           
            RuleFor(x => x.Password).NotNull()
                .WithMessage("Password can not be null")
                .MinimumLength(6)
                .WithMessage("Password can not be less than 6 character")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
                .Matches(@"\W").WithMessage("Password must contain at least one special character");

            RuleFor(x=>x.PasswordConfirmed)
            .Equal(x => x.Password)
            .WithMessage("Password confirmation does not match the password");

            RuleFor(x => x.Phone)
                .Matches(@"^(05\d{9})$").WithMessage("Phone number must be in the format 05X XXX XX XX");
          


        }
    }
}
