namespace WholeSaler.Web.Areas.Admin.Models.ViewModels.User
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<RoleVM> Roles { get; set; }
    }
}
