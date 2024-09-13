using Microsoft.AspNetCore.Mvc;

namespace WholeSaler.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            
            return RedirectToAction("Login", "User", new {returnUrl=returnUrl});
        }
    }
}
