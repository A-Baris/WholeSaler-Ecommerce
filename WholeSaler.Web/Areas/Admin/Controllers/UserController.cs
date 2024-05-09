using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.User;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
           _userManager = userManager;
           _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserVM>();

            foreach (var user in users)
            {
                var userInfo = new UserVM
                {
                    Id = user.Id.ToString(),
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = new List<RoleVM>() 
                };

                foreach (var userRole in await _userManager.GetRolesAsync(user))
                {
                   
                    var role = await _roleManager.FindByNameAsync(userRole);
                    userInfo.Roles.Add(new RoleVM { Id = role.Id.ToString(), Name = role.Name });
                }

                userViewModels.Add(userInfo);
            }
            return View(userViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> AssignRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.UserInfo = user;
            ViewBag.Uid = userId;
            var roles = _roleManager.Roles.ToList();
           var userRoles = await _userManager.GetRolesAsync(user);
            var roleViewModel = new List<RoleAssignVM>();
            foreach (var role in roles)
            {
                var assignRoleVM = new RoleAssignVM()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name
                };

                if (userRoles.Contains(role.Name))
                {
                    assignRoleVM.Exist = true;
                }
                roleViewModel.Add(assignRoleVM);
            }

            return View(roleViewModel);




        }

        [HttpPost]
        public async Task<IActionResult> AssignRoles(string userId, List<RoleAssignVM> requestList)
        {

            var userToRole = await _userManager.FindByIdAsync(userId);

            if (userToRole == null)
            {

                return RedirectToAction("index", "user", new { area = "admin" });
            }

            foreach (var role in requestList)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(userToRole, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userToRole, role.Name);
                }
            }

            return RedirectToAction("index");
        }

    }
    }

