using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
           _roleManager = roleManager;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            AppRole newRole = new AppRole()
            {
                Name = roleName,
            };
            var result =await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                TempData["successMessage"] = "";
                return RedirectToAction("create");
            }
            TempData["errorMessage"] = "";
            return RedirectToAction("create");

        }
    }
}
