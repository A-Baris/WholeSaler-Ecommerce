using Microsoft.AspNetCore.Mvc.Rendering;

namespace WholeSaler.Web.Areas.Admin.Models.ViewModels.User
{
    public class UserRoleVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleVM> Roles { get; set; }
    }
}
