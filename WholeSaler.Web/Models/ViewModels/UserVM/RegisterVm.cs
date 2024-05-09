using WholeSaler.Web.Models.ViewModels.UserVM;

namespace WholeSaler.Web.Models.ViewModels
{
    public class RegisterVm
    {
  
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public string? Phone { get; set; }
        public List<UserAdressVM>? Address { get; set; }
    }
}
