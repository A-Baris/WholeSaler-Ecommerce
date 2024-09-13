using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Store;
using WholeSaler.Web.FluentValidation.Configs;
using WholeSaler.Web.FluentValidation.Validators.User;
using WholeSaler.Web.Helpers.EmailActions;
using WholeSaler.Web.Helpers.HttpClientApiRequests;
using WholeSaler.Web.Models.Entity;
using WholeSaler.Web.Models.ViewModels;
using WholeSaler.Web.Models.ViewModels.Message;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using WholeSaler.Web.Models.ViewModels.UserVM;
using WholeSaler.Web.MongoIdentity;
using static System.Net.WebRequestMethods;
namespace WholeSaler.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpApiRequest _httpApiRequest;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IValidationService<RegisterVm> _registerValidation;
        private readonly IValidationService<LoginVM> _loginValidation;
        private readonly string userApiUri = "https://localhost:7185/api/user";
        public UserController(IHttpApiRequest httpApiRequest,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IValidationService<RegisterVm> registerValidation, IValidationService<LoginVM> loginValidation)
        {
            _httpApiRequest = httpApiRequest;
            _userManager = userManager;
            _signInManager = signInManager;
            _registerValidation = registerValidation;
            _loginValidation = loginValidation;
            
        }
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl)
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
                if (result.Succeeded)
                {
                    var getAccessTokenUri = userApiUri+"/login";
                    var responseToken = await _httpApiRequest.PostAsync(getAccessTokenUri, loginVM);
                    if (responseToken.IsSuccessStatusCode)
                    {
                      var tokenData =  await responseToken.Content.ReadFromJsonAsync<TokenVM>();
                        Response.Cookies.Append("AccessToken", tokenData.AccessToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = tokenData.AccessTokenExpiration
                        });
                        Response.Cookies.Append("RefreshToken", tokenData.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = tokenData.RefreshTokenExpiration
                        });
                    }
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect($"https://localhost:7189{returnUrl}");
                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect($"https://localhost:7189/{returnUrl}");
                    }
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var visitorId = Request.Cookies["visitor"];
                    var checkVisitorCartUri = $"https://localhost:7185/api/shoppingcart/getcart/{visitorId}";
                    var responseVisitorCart = await _httpApiRequest.GetAsync(checkVisitorCartUri);
                    if (responseVisitorCart.IsSuccessStatusCode)
                    {
                        var visitorshoppingCart = await _httpApiRequest.DeserializeJsonToModelForSingle<ShoppingCartUpdateVM>(responseVisitorCart);
                        if (visitorshoppingCart != null)
                        {
                            var checkCartLoginUserUri = $"https://localhost:7185/api/shoppingcart/getcart/{userId}";
                            var userCart = await _httpApiRequest.GetAsync(checkCartLoginUserUri);
                            if (userCart.IsSuccessStatusCode)
                            {
                                var jsonStringForUser = await userCart.Content.ReadAsStringAsync();
                                var shoppingCartForUser = await _httpApiRequest.DeserializeJsonToModelForSingle<ShoppingCartUpdateVM>(userCart);
                                if (shoppingCartForUser != null)
                                {
                                    foreach (var item in visitorshoppingCart.Products)
                                    {
                                        shoppingCartForUser.Products.Add(item);
                                    }
                                    var userCartUpdate = "https://localhost:7185/api/shoppingcart/edit";
                                    var resultUpdateUserCart = await _httpApiRequest.PutAsync(userCartUpdate, shoppingCartForUser);
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            else
                            {
                                var cartUpdateUri = "https://localhost:7185/api/shoppingcart/edit";
                                visitorshoppingCart.UserId = userId;
                                var resultUpdateCart = await _httpApiRequest.PutAsync(cartUpdateUri, visitorshoppingCart);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    if (User.IsInRole("admin"))
                    {
                        return RedirectToAction("index", "user", new { area = "admin" });
                    }
                    var username = User.Identity.Name;
                    TempData["LoginSuccess"] = $"Welcome {username}";
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.ErrorMessage = "Email or Password is wrong!";
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
            var registerApiUri = userApiUri+"/register";
            var response = await _httpApiRequest.PostAsync(registerApiUri,registerVm);
            if (response.IsSuccessStatusCode) 
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
            try
            {
                await _signInManager.SignOutAsync();
                Response.Cookies.Delete("AccessToken");
                Response.Cookies.Delete("RefreshToken");
                return RedirectToAction("index", "home");
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = "An unexpected error, please try again";
                return RedirectToAction("index", "home");
            }
          
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                var changedPassword = await _userManager.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
                if (changedPassword.Succeeded)
                {
                    TempData["SuccessMessage"] = "Password is changed successfully.\n Please login with new password";
                    await _signInManager.SignOutAsync();
                    Response.Cookies.Delete("AccessToken");
                    Response.Cookies.Delete("RefreshToken");
                    return RedirectToAction("login", "user");
                }
                TempData["ErrorMessage"]= "An unexpected error, please try again";
                return View(changedPassword);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error, please try again";
                return View();
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
            [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user==null)
            {
                TempData["ErrorMessage"] = $"{email} adresi ile kayıtlı kullanıcı bulunamadı";
                return RedirectToAction("ForgotPassword","user");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            string resetLink = Url.Action("ResetPassword", "user", new { userId = user.Id, token = encodedToken }, Request.Scheme);
            EmailSender.SendEMail(email, "Şifremi Unuttum", $"Şifre değiştirmek için linke tıklayınız:\n {resetLink}");
            TempData["SuccessMessage"] = $"{email} adresine şifre yenileme linki başarıyla gönderilmiştir";
            return RedirectToAction("ForgotPassword", "user");
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            var resetVM = new ResetPasswordVM
            {
                UserId = userId,
                Token = token
            };
            return View(resetVM);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetVM)
        {
            var user = await _userManager.FindByIdAsync(resetVM.UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Hatalı veya eksik erişim isteğiyle karşılaşıldı";
                return RedirectToAction("index", "Home");
            }
            var decodedToken = WebUtility.UrlDecode(resetVM.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetVM.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifre yenileme başarıyla gerçekleşmiştir.\n Bilgilerinizle tekrar giriş yapabilirsiniz";
                return RedirectToAction("login", "user");
            }
            else
            {
                TempData["ErrorMessage"] = "Beklenmedik bir hatayla karşılaşıldı lütfen tekrar deneyiniz veya yeniden şifre yenileme isteği gerçekleştiriniz";
                return View(resetVM);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mystore()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var user =  await _userManager.FindByEmailAsync(userEmail);
            if (user.StoreId != null)
            {
                var checkStoreUri = $"https://localhost:7185/api/store/{user.StoreId}";
                var response = await _httpApiRequest.GetAsync(checkStoreUri);
                if (response.IsSuccessStatusCode) 
                {
                    var storeJson = await response.Content.ReadAsStringAsync();
                    var storeData = JsonConvert.DeserializeObject<StoreVM>(storeJson);
                    if (storeData.AdminConfirmation == Areas.Admin.Models.Enums.AdminConfirmation.Accepted) 
                    {
                        return RedirectToAction("index", "store", new { area = "auth" });
                    }
                }
                return RedirectToAction("applicationdetails","store");
            }
            else
            {
                var getStoresUri = "https://localhost:7185/api/store";
                var storeResponse = await _httpApiRequest.GetAsync(getStoresUri);
                if (storeResponse.IsSuccessStatusCode)
                {
                    var stores = await _httpApiRequest.DeserializeJsonToModelForList<StoreVM>(storeResponse);
                    var userStore = stores.Where(x=>x.UserId==user.Id.ToString()).FirstOrDefault();
                    return RedirectToAction("applicationdetails", "store");
                }
            }
            return RedirectToAction("create", "store");
        }
        //Message
        [HttpGet]
        public async Task<IActionResult> SendMessage(string receiverId,string receiverName)
        {
            var receiverModel = (receiverId:receiverId, receiverName:receiverName);
            ViewData["ReceiverId"]= receiverModel.receiverId;
            ViewData["ReceiverName"]= receiverModel.receiverName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageVM sendMessageVM)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            sendMessageVM.SenderName = user.UserName;
            sendMessageVM.SenderId = userId.ToString();
            var messageCreateUri = "https://localhost:7185/api/message/create";
            var response = await _httpApiRequest.PostAsync(messageCreateUri, sendMessageVM);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "home");
            }
            return View(sendMessageVM);
        }
        [HttpGet]
        public async Task<IActionResult>MessageBox()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.UserName = user.UserName;
            var receivedMessageUri = $"https://localhost:7185/api/message/messagebox/{user.UserName}/{user.Id}";
            var response = await _httpApiRequest.GetAsync(receivedMessageUri);
            if (response.IsSuccessStatusCode) 
            { 
            var jsonData = await response.Content.ReadAsStringAsync();
            //var senders = JsonConvert.DeserializeObject<List<string>>(jsonData);
                var senders = await _httpApiRequest.DeserializeJsonToModelForList<string>(response);
                return View(senders);
            }
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public async Task<IActionResult>ReceivedMessageFrom(string sender)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var receivedMessageUri = $"https://localhost:7185/api/message/receivedMessagefrom/{sender}/{user.UserName}";
            var response = await _httpApiRequest.GetAsync(receivedMessageUri);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.ReceiverName = sender;
                var receiverUser = await _userManager.FindByNameAsync(sender);
                ViewBag.ReceiverId = receiverUser.Id.ToString();
                var jsonData = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<List<ReceivedMessageVM>>(jsonData);
                return PartialView(messages);
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        public async Task<IActionResult> AddAdress([FromBody] UserAddressCreateVm adressVM)
        {
          
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEditApiUri = userApiUri + $"/edit/{userId}";
            var response = await _httpApiRequest.PutAsync(userEditApiUri, adressVM);
            return View();
        }
    }
}
