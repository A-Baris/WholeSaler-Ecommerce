using Microsoft.AspNetCore.Mvc;

namespace WholeSaler.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            
            return RedirectToAction("Login", "User");
        }
    }
}
