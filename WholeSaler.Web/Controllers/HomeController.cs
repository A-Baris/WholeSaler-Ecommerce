using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Models;

using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;


namespace WholeSaler.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();


        }

        public async Task<IActionResult> Index(string productName, string categoryName)
        {

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            var categoryUri = "https://localhost:7185/api/category";
            var responseCategory = await _httpClient.GetAsync(categoryUri);



            var apiUri = "https://localhost:7185/api/product";
            var response = await _httpClient.GetAsync(apiUri);
            if (response.IsSuccessStatusCode)
            {


                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductForCartVM>>(jsonString);
                if (responseCategory.IsSuccessStatusCode)
                {
                    var jsonCategory = await responseCategory.Content.ReadAsStringAsync();
                    var categoryData = JsonConvert.DeserializeObject<List<CategoryVM>>(jsonCategory);
                    ViewBag.Categories = categoryData;
                    if (categoryName != null)
                    {
                        var productForCategory = data.Where(x => x.Category.CategoryName == categoryName).ToList();
                        return View(productForCategory);
                    }

                }

                if (!string.IsNullOrEmpty(productName))
                {

                    var lowerCasePrefix = productName.Substring(0, Math.Min(3, productName.Length)).ToLower();

                    data = data.Where(x => x.Name.ToLower().StartsWith(lowerCasePrefix)).ToList();
                    if (data.Count > 0) { return View(data); }
                    else
                    {
                        TempData["productNameMessage"] = $"{productName} is not found";
                        return RedirectToAction("index");
                    }


                }
                return View(data);
            }
            return View();

        }


        //[HttpGet]
        //public  IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login(UserLoginVM loginVM)
        //{


        //    var apiUrl = "https://localhost:7185/api/user/login";
        //    var json = JsonConvert.SerializeObject(loginVM);
        //    var content = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
        //    var response = await _httpClient.PostAsync(apiUrl, content);

        //    if(response.IsSuccessStatusCode)
        //    {

        //        // API'den gelen yanýtý oku
        //        var responseBody = await response.Content.ReadAsStringAsync();

        //        // Yanýt içindeki JWT token'ý al

        //        var token = responseBody;

        //        // Token'ý iþle (örneðin, token'ý JWT kütüphanesi kullanarak iþleyebilirsiniz)
        //        var handler = new JwtSecurityTokenHandler();
        //        var jwtToken = handler.ReadJwtToken(token);

        //        // JWT token içindeki talepleri al ve ClaimsIdentity oluþtur
        //        var userClaims = jwtToken.Claims;
        //        var identity = new ClaimsIdentity(userClaims, "ApiAuth");

        //        // ClaimsIdentity üzerinden ClaimsPrincipal oluþtur ve HttpContext.User'a ata
        //        var principal = new ClaimsPrincipal(identity);
        //        HttpContext.User = principal;








        //        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
        //        var user2 = User.Identity.Name;

        //        var visitorId = Request.Cookies["visitor"];
        //        var checkCartUri = $"https://localhost:7185/api/shoppingcart/getcart/{visitorId}";

        //        try
        //        {
        //            var result = await _httpClient.GetAsync(checkCartUri);
        //            var jsonString = await result.Content.ReadAsStringAsync();
        //            var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonString);
        //            if (shoppingCart != null)
        //            {
        //                var checkCartLoginUserUri = $"https://localhost:7185/api/shoppingcart/getcart/{userId}";
        //                var userCart = await _httpClient.GetAsync(checkCartLoginUserUri);
        //                var jsonStringForUser = await userCart.Content.ReadAsStringAsync();
        //                var shoppingCartForUser = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonStringForUser);
        //                if (shoppingCartForUser != null)
        //                {
        //                    foreach (var item in shoppingCart.Products)
        //                    {
        //                        shoppingCartForUser.Products.Add(item);

        //                    }
        //                    var userCartUpdate = "https://localhost:7185/api/shoppingcart/edit";
        //                    var jsonCartUpdate = JsonConvert.SerializeObject(shoppingCartForUser);
        //                    var contentCartUpdate = new StringContent(jsonCartUpdate, System.Text.Encoding.UTF8, "application/json");
        //                    var resultUpdateUserCart = await _httpClient.PutAsync(userCartUpdate, contentCartUpdate);
        //                    return RedirectToAction("Index", "Home");
        //                }
        //                shoppingCart.UserId = userId;

        //                var cartUpdateUri = "https://localhost:7185/api/shoppingcart/edit";
        //                var jsonUpdate = JsonConvert.SerializeObject(shoppingCart);
        //                var contentUpdate = new StringContent(jsonUpdate, System.Text.Encoding.UTF8, "application/json");
        //                var resultUpdateCart = await _httpClient.PutAsync(cartUpdateUri, contentUpdate);
        //                return RedirectToAction("Index", "Home");
        //            }
        //        } catch (Exception ex) { Console.WriteLine(ex.Message); }



        //        return RedirectToAction("Index", "Home");


        //    }



        //    return View("login");
        //}

        //public IActionResult Logout()
        //{
        //    Response.Cookies.Delete("access_token");


        //    return RedirectToAction("Index", "Home");
        //}



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
