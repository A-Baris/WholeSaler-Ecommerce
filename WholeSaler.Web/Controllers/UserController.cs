using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Security.Claims;
using WholeSaler.Web.FluentValidation.Configs;
using WholeSaler.Web.FluentValidation.Validators.User;
using WholeSaler.Web.Helpers.EmailActions;
using WholeSaler.Web.Models.Entity;
using WholeSaler.Web.Models.ViewModels;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using WholeSaler.Web.Models.ViewModels.UserVM;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly IValidationService<RegisterVm> _registerValidation;
        private readonly IValidationService<LoginVM> _loginValidation;
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IValidationService<RegisterVm> registerValidation, IValidationService<LoginVM> loginValidation)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _signInManager = signInManager;

            _registerValidation = registerValidation;
            _loginValidation = loginValidation;
            _httpClient = _httpClientFactory.CreateClient();

        }


        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM,string returnUrl)
        {

            var errors = _loginValidation.GetValidationErrors(loginVM);
            if (errors.Any())
            {
                ModelStateHelper.AddErrorsToModelState(ModelState, errors);

                return View(loginVM);
            }
          


            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser != null)
            {
                var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, false, false);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect("~/"+returnUrl);
                }

                if (result.Succeeded)
                {
                    
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var user = await _userManager.FindByIdAsync(userId);
              
                    var visitorId = Request.Cookies["visitor"];
                    var checkVisitorCartUri = $"https://localhost:7185/api/shoppingcart/getcart/{visitorId}";

                    var responseVisitorCart = await _httpClient.GetAsync(checkVisitorCartUri);
                    if (responseVisitorCart.IsSuccessStatusCode)
                    {
                        var jsonString = await responseVisitorCart.Content.ReadAsStringAsync();
                        var visitorshoppingCart = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonString);
                        if (visitorshoppingCart != null)
                        {
                            var checkCartLoginUserUri = $"https://localhost:7185/api/shoppingcart/getcart/{userId}";
                            var userCart = await _httpClient.GetAsync(checkCartLoginUserUri);
                            if (userCart.IsSuccessStatusCode)
                            {
                                var jsonStringForUser = await userCart.Content.ReadAsStringAsync();
                                var shoppingCartForUser = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonStringForUser);
                                if (shoppingCartForUser != null)
                                {
                                    foreach (var item in visitorshoppingCart.Products)
                                    {
                                        shoppingCartForUser.Products.Add(item);

                                    }
                                    var userCartUpdate = "https://localhost:7185/api/shoppingcart/edit";

                                    var jsonCartUpdate = JsonConvert.SerializeObject(shoppingCartForUser);
                                    var contentCartUpdate = new StringContent(jsonCartUpdate, System.Text.Encoding.UTF8, "application/json");
                                    var resultUpdateUserCart = await _httpClient.PutAsync(userCartUpdate, contentCartUpdate);
                                    return RedirectToAction("Index", "Home");
                                }
                               
                            }
                            else
                            {
                                var cartUpdateUri = "https://localhost:7185/api/shoppingcart/edit";
                                visitorshoppingCart.UserId = userId;
                                var jsonUpdate = JsonConvert.SerializeObject(visitorshoppingCart);
                                var contentUpdate = new StringContent(jsonUpdate, System.Text.Encoding.UTF8, "application/json");
                                var resultUpdateCart = await _httpClient.PutAsync(cartUpdateUri, contentUpdate);
                                return RedirectToAction("Index", "Home");

                            }

                        }
                    }


                    if (User.IsInRole("admin"))
                    {
                       
                        return RedirectToAction("index", "user", new {area="admin"});
                    }
                    var username = User.Identity.Name;
                    TempData["LoginSuccess"] = $"Welcome {username}";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            var errors = _registerValidation.GetValidationErrors(registerVm);
            if (errors.Any())
            {
                ModelStateHelper.AddErrorsToModelState(ModelState, errors);
                return View(registerVm);
            }
            var existingEmail = await _userManager.FindByEmailAsync(registerVm.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "This email is already using by another user");
                return View(registerVm);
            }
            var existingUsername = await _userManager.FindByNameAsync(registerVm.Username);
            if (existingUsername != null)
            {
                ModelState.AddModelError("Username", "This username is already using by another user");
                return View(registerVm);
            }
            AppUser appUser = new AppUser
            {
                UserName = registerVm.Username,
                Email = registerVm.Email,
                PhoneNumber = registerVm.Phone,


            };
            var result = await _userManager.CreateAsync(appUser, registerVm.Password);
            if (result.Succeeded)
            {
                //var mailBody = $"Dear, {registerVm.Username} \nYou completed the registration.Now, you are already ready to enjoy and do shopping in the right address.";
                //EmailSender.SendEMail(registerVm.Email, "Welcome to WholeSaller", mailBody);
                TempData["registerSuccess"] = "Registered successfully";
                return RedirectToAction("Login", "User");
            }

            TempData["ErrorMessage"] = "An unexpected error, please try again";
            return View(registerVm);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            

            return RedirectToAction("index", "home");
        }


    }
}
