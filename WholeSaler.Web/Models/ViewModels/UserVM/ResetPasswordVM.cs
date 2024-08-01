using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WholeSaler.Web.Models.ViewModels.UserVM
{
    public class ResetPasswordVM
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        [DisplayName("Şifre")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
